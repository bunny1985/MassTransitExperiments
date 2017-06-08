using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RxTest.Controllers
{
    public class MsgModel
    {
        public MsgModel()
        {
            this.date = DateTime.Now.ToShortTimeString();
        }

        public string msg { get; set; }
        public string date { get; set; }
    }

    [RoutePrefix("rx")]
    public class RxController : ApiController
    {
        private static ConcurrentDictionary<string, Stream> clients = new ConcurrentDictionary<string, Stream>();

        [HttpGet]
        [Route("subscribe")]
        public async Task<HttpResponseMessage> Subscribe(HttpRequestMessage request)
        {
            var response = request.CreateResponse();
            response.Content = new PushStreamContent(async (a, b, c) => { await OnStreamAvailable(a, b, c); }, "text/event-stream");

            return response;
        }

        private static async Task WriteEventToStreamAsync(Guid id, string type, string data, Stream stream)
        {
            try
            {
                StreamWriter sw = new StreamWriter(stream);

                await sw.WriteAsync("event: " + type + "\n");
                await sw.FlushAsync();
                await sw.WriteAsync("id: " + Guid.NewGuid() + "\n");
                await sw.FlushAsync();
                await sw.WriteAsync("data: " + data + "\n\n");
                await sw.FlushAsync();
                await sw.WriteAsync("\n");
                await sw.FlushAsync();
                await stream.FlushAsync();
                await sw.FlushAsync();
                await stream.FlushAsync();
            }
            catch (Exception e)
            {
                Stream ignore;
                //clients.TryRemove( out ignore);
            }
        }

        [HttpPost]
        [Route("msg")]
        public async Task<IHttpActionResult> Push(MsgModel model)
        {
            foreach (var clientPair in clients)
            {
                var client = clientPair.Value;
                try
                {
                    await WriteEventToStreamAsync(Guid.NewGuid(), "push", JsonConvert.SerializeObject(model), client);
                }
                catch (Exception)
                {
                    Stream ignore;
                    clients.TryRemove(clientPair.Key, out ignore);
                }
            }
            return Ok();
        }

        private async Task OnStreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            await WriteEventToStreamAsync(Guid.NewGuid(), "welcome", JsonConvert.SerializeObject(new MsgModel() { msg = "Welcome" }), stream);
            var userName = this.User.Identity.Name;
            Stream ignore;
            clients.TryRemove(userName, out ignore);
            clients.TryAdd(this.User.Identity.Name, stream);
        }
    }
}