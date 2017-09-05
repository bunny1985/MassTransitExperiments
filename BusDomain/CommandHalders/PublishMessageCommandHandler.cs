using BusDomain.Agregates;
using BusDomain.Commnads;
using BusDomain.Identities;
using EventFlow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BusDomain.CommandHalders
{
    public class PublishMessageCommandHandler : CommandHandler<MessagesStreamAggregate, BasicIdentity, PublishMessageCommand>
    {
        public override async Task ExecuteAsync(MessagesStreamAggregate aggregate, PublishMessageCommand command, CancellationToken cancellationToken)
        {
            aggregate.BroadCastMessage(command.msg);
        }
    }
}