using System;
using System.Threading.Tasks;

namespace bb.bus
{
    public interface IEventsStore
    {
        Task StoreAsync(Object obj);
    }
}