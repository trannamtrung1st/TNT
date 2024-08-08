using System;
using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Http.Configurations;
using TNT.Boilerplates.Http.Handlers;

namespace TNT.Boilerplates.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            return services.AddScoped<ErrorResponseWrapHandler>();
        }

        public static IServiceCollection AddOpenIdClientCredentials(this IServiceCollection services,
            Action<OpenIdClientCredentialsHandlerOptions> configure)
        {
            return services.AddScoped<OpenIdClientCredentialsHandler>()
                .Configure(configure);
        }
    }
}
