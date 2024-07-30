using System;
using Microsoft.Extensions.Configuration;
using TNT.Layers.Services.Configurations;

namespace TNT.Layers.Services.Extensions
{
    public static class ConfigurationExtensions
    {
        public static OpenIdInfo GetOpenIdInfo(this IConfiguration configuration,
            string configKey = nameof(OpenIdInfo), bool useDefaults = true, bool withPrefix = true)
        {
            var openIdInfo = configuration.GetSection(configKey).Get<OpenIdInfo>()
                ?? throw new ArgumentNullException(paramName: configKey);

            if (useDefaults)
            {
                if (withPrefix)
                    openIdInfo.UseDefaultsWithPrefix();
                else openIdInfo.UseDefaults();
            }

            return openIdInfo;
        }
    }
}
