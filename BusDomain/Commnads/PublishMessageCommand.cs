using BusDomain.Agregates;
using BusDomain.Identities;
using EventFlow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusDomain.Commnads
{
    public class PublishMessageCommand : Command<MessagesStreamAggregate, BasicIdentity>
    {
        public PublishMessageCommand(BasicIdentity id, string msg) : base(id)
        {
            this.msg = msg;
        }

        public string msg { get; set; }
    }
}