using System.Collections.Concurrent;
using Serilog.Context;
using TNT.Layers.Service.Services.Abstracts;

namespace TNT.Layers.Service.Services
{
    public class DefaultRequestContext : IRequestContext
    {
        protected readonly ConcurrentDictionary<string, object> internalData;
        public DefaultRequestContext()
        {
            internalData = new();
        }

        public virtual object Get(string key) => internalData[key];

        public virtual void Set(string key, object value)
        {
            internalData[key] = value;
            LogContext.PushProperty(key, value);
        }
    }
}
