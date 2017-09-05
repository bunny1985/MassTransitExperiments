using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusDomain.Identities
{
    public class BasicIdentity : Identity<BasicIdentity>
    {
        public BasicIdentity(string value) : base(value)
        {
        }
    }
}