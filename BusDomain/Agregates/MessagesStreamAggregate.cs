using BusDomain.Events;
using BusDomain.Identities;
using EventFlow.Aggregates;
using System;
using System.Collections.Generic;
using WebSocketHandling;

namespace BusDomain.Agregates
{
    public class MessagesStreamAggregate : AggregateRoot<MessagesStreamAggregate, BasicIdentity>
    {
        public MessagesStreamAggregate(BasicIdentity id, IMessageDispatcher messageDispatcher) : base(id)
        {
            _messageDispatcher = messageDispatcher;
            _messages = new List<string>();
        }

        private List<string> _messages;
        private readonly IMessageDispatcher _messageDispatcher;

        public IEnumerable<String> Messsages { get { return _messages; } }

        public void Apply(MessagePublishedEvent domainEvent)
        {
        }

        public void BroadCastMessage(string message)
        {
            var webSocketMessage = new Message() { Content = message };

            _messages.Add(message);
            _messageDispatcher.PublishAsync(webSocketMessage).Wait();
            Emit(new MessagePublishedEvent(message));
        }
    }
}