using MassTransit.Audit;
using System;

namespace bb.bus
{
    public class AuditEntry : MessageAuditMetadata
    {
        public string MessageAsJson { get; set; }
        public string MessageType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}