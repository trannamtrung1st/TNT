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
            var clusterMap = new Dictionary<string, ClusterConfig>();

            foreach (var kvp in options.Mappings)
            {
                var mapping = kvp.Value;
                var routeId = kvp.Key;
                var path = mapping.Route;
                if (!mapping.ExactMatch)
                    path = Path.Combine(path, "{**catch-all}");
                if (!clusterMap.TryGetValue(mapping.Destination, out var clusterConfig))
                {
                    clusterConfig = new ClusterConfig
                    {
                        ClusterId = mapping.Destination,
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            [DefaultRootDestination] = new DestinationConfig { Address = mapping.Destination }
                        }
                    };
                    clusters.Add(clusterConfig);
                    clusterMap[mapping.Destination] = clusterConfig;
                }
                var routeConfig = mapping.Config ?? options.DefaultRouteConfig;
                var route = new RouteConfig
                {
                    RouteId = routeConfig?.RouteId ?? routeId,
                    ClusterId = routeConfig?.ClusterId ?? clusterConfig.ClusterId,
                    Match = routeConfig?.Match ?? new RouteMatch { Path = path },
                    AuthorizationPolicy = routeConfig?.AuthorizationPolicy,
                    CorsPolicy = routeConfig?.CorsPolicy,
                    MaxRequestBodySize = routeConfig?.MaxRequestBodySize,
                    Metadata = routeConfig?.Metadata,
                    Order = routeConfig?.Order,
                    Transforms = routeConfig?.Transforms,
                };
                routes.Add(route);
            }

            return builder.LoadFromMemory(routes, clusters);
        }
    }
}