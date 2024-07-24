using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Logging.Interceptors;

namespace TNT.Boilerplates.Logging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingDefaults(this IServiceCollection services)
        {
            return services.AddScoped<MethodLoggingInterceptor>()
                .AddScoped<AttributeLoggingInterceptor>();
        }
    }
}
