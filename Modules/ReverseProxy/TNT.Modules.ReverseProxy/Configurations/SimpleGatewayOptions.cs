using System.Collections.Generic;

namespace TNT.Modules.ReverseProxy.Configurations
{
    public class SimpleGatewayOptions
    {
        public IReadOnlyDictionary<string, string> Mapping { get; set; }
    }
}