using MassTransit;
using System;

namespace BB.Bus.Messages
{
    public abstract class Message : CorrelatedBy<string>
    {
        public Message(string correlationId, string name)
        {
            Name = name;
            CorrelationId = correlationId;
        }

        public Message(string name)
        {
            Name = name;
            CorrelationId = Guid.NewGuid().ToString();
        }

        public string Name { get; private set; }
        public string CorrelationId { get; set; }
    }
}