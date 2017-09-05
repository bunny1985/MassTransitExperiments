using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketHandling
{
    public class WebSocketClient : WebSocketHandler
    {
        private readonly string _userName;
        private readonly WebSocketCollection _containingCollection;

        public WebSocketClient(string userName, WebSocketCollection containingCollection)
        {
            _userName = userName;
            _containingCollection = containingCollection;
        }

        public override void OnClose()
        {
            _containingCollection.Remove(this);
            base.OnClose();
        }
    }
}