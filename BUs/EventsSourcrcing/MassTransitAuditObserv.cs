using MassTransit;
using MassTransit.Audit;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace bb.bus
{
    /// <summary>
    /// Default Auditor - it stores Audit Entries to IEventsStore
    /// It uses IEventsStore internaly
    /// </summary>
    internal class MessageAuditStoreWithEventsStore : IMessageAuditStore
    {
        private readonly IEventsStore _eventsStore;

        public MessageAuditStoreWithEventsStore(IEventsStore evenstStore)
        {
            _eventsStore = evenstStore;
        }

        public async Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            var entry = new AuditEntry()
            {
                ContextType = metadata.ContextType,
                ConversationId = metadata.ConversationId,
                CorrelationId = metadata.CorrelationId,
                Custom = metadata.Custom,
                DestinationAddress = metadata.DestinationAddress,
                FaultAddress = metadata.FaultAddress,
                Headers = metadata.Headers,
                InitiatorId = metadata.InitiatorId,
                MessageId = metadata.MessageId,
                RequestId = metadata.RequestId,
                ResponseAddress = metadata.ResponseAddress,
                SourceAddress = metadata.SourceAddress,
                //Styore message data
                MessageAsJson = JsonConvert.SerializeObject(message),
                MessageType = message.GetType().ToString(),
                CreatedAt = DateTime.Now
            };

            await _eventsStore.StoreAsync(entry);
        }
    }
}