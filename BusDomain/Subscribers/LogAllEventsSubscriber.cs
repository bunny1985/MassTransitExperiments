using EventFlow.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using System.Threading;
using System.Diagnostics;

namespace BusDomain.Subscribers
{
    public class LogAllEventsSubscriber : ISubscribeSynchronousToAll
    {
        public Task HandleAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Event Occured");
            return Task.FromResult(0);
        }
    }
}