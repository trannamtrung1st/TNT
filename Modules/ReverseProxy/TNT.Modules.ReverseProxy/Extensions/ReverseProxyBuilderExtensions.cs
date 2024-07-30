using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TNT.Modules.ReverseProxy.Configurations;
using Yarp.ReverseProxy.Configuration;

namespace TNT.Modules.ReverseProxy.Extensions
{
    public static class ReverseProxyBuilderExtensions
    {
        public const string DefaultRootDestination = "root";

        public static IReverseProxyBuilder LoadSimpleGateway(this IReverseProxyBuilder builder, IConfiguration section)
        {
            var options = section.Get<SimpleGatewayOptions>();
            return builder.LoadSimpleGateway(options);
        }

        public static IReverseProxyBuilder LoadSimpleGateway(this IReverseProxyBuilder builder, SimpleGatewayOptions options)
        {
            var routes = new List<RouteConfig>();
            var clusters = new List<ClusterConfig>();

            foreach (var mapping in options.Mapping)
            {
                var routeId = mapping.Key;
                var clusterId = mapping.Key;
                var prefix = mapping.Key;
                var path = Path.Combine(prefix, "{**catch-all}");
                var destinationAddress = mapping.Value;
                routes.Add(new RouteConfig
                {
                    RouteId = routeId,
                    ClusterId = clusterId,
                    Match = new RouteMatch { Path = path },
                    Transforms = options.DefaultTransforms
                });
                clusters.Add(new ClusterConfig
                {
                    ClusterId = clusterId,
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        [DefaultRootDestination] = new DestinationConfig { Address = destinationAddress }
                    }
                });
            }

            return builder.LoadFromMemory(routes, clusters);
        }
    }
}