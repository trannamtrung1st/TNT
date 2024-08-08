using System;
using Microsoft.Extensions.Configuration;
using TNT.Layers.Infrastructure.Auth.Configurations;

namespace TNT.Layers.Services.Extensions
{
    public static class ConfigurationExtensions
    {
        public static OpenIdInfo GetOpenIdInfo(this IConfiguration configuration,
            string configKey = nameof(OpenIdInfo), bool useDefaults = true,
            Action<OpenIdInfo> extraConfigure = null)
        {
            var openIdInfo = configuration.GetSection(configKey).Get<OpenIdInfo>()
                ?? throw new ArgumentNullException(paramName: configKey);

            if (useDefaults)
                openIdInfo.UseDefaults();

            extraConfigure?.Invoke(openIdInfo);

            return openIdInfo;
        }
    }
}
