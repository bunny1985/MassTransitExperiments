using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Bus.Messages
{
    public interface CorrelatedBy<out TKey>
    {
        //
        // Summary:
        //     Returns the CorrelationId for the message
        TKey CorrelationId { get; }
    }
}