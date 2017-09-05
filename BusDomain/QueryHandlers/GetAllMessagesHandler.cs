using BusDomain.Queries;
using BusDomain.ReadModels;
using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BusDomain.QueryHandlers
{
    internal class GetAllMessagesHandler : IQueryHandler<GetAllMessages, MessagesList>
    {
        public Task<MessagesList> ExecuteQueryAsync(GetAllMessages query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new MessagesList());
        }
    }
}