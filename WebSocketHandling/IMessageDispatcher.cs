using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketHandling
{
    public interface IMessageDispatcher
    {
        Task PublishAsync(Message message);

        Task SendAsync(Message message, string recivier);

        Task<WebSocketClient> JoinClientAsync(string userName);

        Task RemoveClientAsync(string userName);
    }
}