﻿using Microsoft.Web.WebSockets;

namespace RxTest.Infrastructure.Websocket
{
    public class TestWebSocketHandler : WebSocketHandler

    {
        private static WebSocketCollection clients = new WebSocketCollection();

        private string name;

        public TestWebSocketHandler(string name)
        {
            this.name = name;
        }

        public override void OnOpen()

        {
            this.name = this.WebSocketContext.QueryString["name"];

            clients.Add(this);

            clients.Broadcast("client  has connected.");
        }

        public override void OnMessage(string message)

        {
            clients.Broadcast(string.Format("{0} said: {1}", name, message));
        }

        public void Publish(string message)

        {
            clients.Broadcast(message); ;
        }

        public override void OnClose()

        {
            clients.Remove(this);

            clients.Broadcast(string.Format("{0} has gone away.", name));
        }
    }
}