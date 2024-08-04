using System;
using Microsoft.Extensions.Configuration;
using TNT.Modules.ReverseProxy.Configurations;

namespace TNT.Modules.ReverseProxy.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebClientDevOptions GetWebClientDev(this IConfiguration configuration, string configKey = "WebClientDev")
        {
            var openIdInfo = configuration.GetSection(configKey).Get<WebClientDevOptions>()
                ?? throw new ArgumentNullException(paramName: configKey);
            return openIdInfo;
        }
    }
}
