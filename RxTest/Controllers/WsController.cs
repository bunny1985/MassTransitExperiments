using Newtonsoft.Json;
using RxTest.Infrastructure.Websocket;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace RxTest.Controllers
{
    public class WsController : ApiController
    {
        private static TestWebSocketHandler _webSocketHandler;

        public WsController()
        {
            if (_webSocketHandler == null)
            {
                _webSocketHandler = new TestWebSocketHandler("WSNAME");
            }
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

        public IHttpActionResult Publish(MsgModel msg)
        {
            var json = JsonConvert.SerializeObject(msg);

            _webSocketHandler.Publish(json);
            return Ok();
        }

        private async Task ProcessWebSocketRequest(AspNetWebSocketContext context)

        {
            var sessionCookie = context.Cookies["SessionId"];

            if (sessionCookie != null)

            {
                var wsHandler = new TestWebSocketHandler(sessionCookie.Value);
                await wsHandler.ProcessWebSocketRequestAsync(context);
            }
        }
    }
}