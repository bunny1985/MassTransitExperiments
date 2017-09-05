using BusDomain.ReadModels;
using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusDomain.Queries
{
    public class GetAllMessages : IQuery<MessagesList>
    {
    }
}