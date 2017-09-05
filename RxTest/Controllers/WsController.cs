using BusDomain.Commnads;
using BusDomain.Identities;
using EventFlow;
using Newtonsoft.Json;
using RxTest.Infrastructure.Websocket;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using WebSocketHandling;

namespace RxTest.Controllers
{
    public class WsController : ApiController
    {
        private static TestWebSocketHandler _webSocketHandler;
        private readonly ICommandBus _commandBus;
        private readonly IMessageDispatcher _messageDipatcher;

        public WsController(ICommandBus commandBus, IMessageDispatcher messageDipatcher)
        {
            _messageDipatcher = messageDipatcher;
            _commandBus = commandBus;
        }

        public async Task<HttpResponseMessage> Get()
        {
            var currentContext = HttpContext.Current;

            return await Task.Run(() =>
            {
                if (currentContext.IsWebSocketRequest || currentContext.IsWebSocketRequestUpgrading)

                {
                    currentContext.AcceptWebSocketRequest(ProcessWebSocketRequest);

                    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            });
        }

        public async Task<IHttpActionResult> Publish(MsgModel msg)
        {
            await _commandBus.PublishAsync(new PublishMessageCommand(BasicIdentity.New, msg.msg), CancellationToken.None);
            return Ok();
        }

        private async Task ProcessWebSocketRequest(AspNetWebSocketContext context)

        {
            var sessionCookie = context.Cookies["SessionId"];

            if (sessionCookie != null)
            {
                var handler = await _messageDipatcher.JoinClientAsync("Zenek");
                await handler.ProcessWebSocketRequestAsync(context);
                //var wsHandler = new TestWebSocketHandler(sessionCookie.Value);
                //await wsHandler.ProcessWebSocketRequestAsync(context);
            }
        }
    }
}