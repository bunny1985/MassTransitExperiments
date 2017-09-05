using Microsoft.Web.WebSockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSocketHandling
{
    public class WebSocketMessageDispatcher : IMessageDispatcher
    {
        private static Dictionary<string, WebSocketCollection> _clients = new Dictionary<string, WebSocketCollection>();

        public Task<WebSocketClient> JoinClientAsync(string userName)
        {
            if (!_clients.ContainsKey(userName))
            {
                _clients.Add(userName, new WebSocketCollection());
            }
            var collecion = _clients[userName];

            var webSocket = new WebSocketClient(userName, collecion);
            collecion.Add(webSocket);

            return Task.FromResult(webSocket);
        }

        public Task PublishAsync(Message message)
        {
            foreach (var collection in _clients)
            {
                collection.Value.Broadcast(string.Format("Someone said: {0}", message.Content));
            }
            return Task.CompletedTask;
        }

        public Task RemoveClientAsync(string userName)
        {
            if (_clients.ContainsKey(userName))
            {
                _clients.Remove(userName);
            }
            return Task.CompletedTask;
        }

        public Task SendAsync(Message message, string recivier)
        {
            if (_clients.ContainsKey(recivier))
            {
                var collecion = _clients[recivier];
                collecion.Broadcast(string.Format("Someone said: {0}", message.Content));
            }
            return Task.CompletedTask;
        }
    }
}