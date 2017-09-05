using BusDomain.Agregates;
using BusDomain.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace BusDomain.Events
{
    [EventVersion("messagePublishedEvent", 1)]
    public class MessagePublishedEvent : AggregateEvent<MessagesStreamAggregate, BasicIdentity>
    {
        public MessagePublishedEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}