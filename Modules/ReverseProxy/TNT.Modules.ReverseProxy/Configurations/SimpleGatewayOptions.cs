using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

namespace TNT.Modules.ReverseProxy.Configurations
{
    public class SimpleGatewayOptions
    {
        public IReadOnlyDictionary<string, SimpleMapping> Mappings { get; set; }
        public RouteConfig DefaultRouteConfig { get; set; }
    }

    public class SimpleMapping
    {
        public string Route { get; set; }
        public string Destination { get; set; }
        public bool ExactMatch { get; set; }
        public RouteConfig Config { get; set; }
    }
}