using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Diagnostic.Abstracts;

namespace TNT.Boilerplates.Diagnostic.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceMonitor(this IServiceCollection services)
        {
            return services.AddSingleton<IResourceMonitor, ResourceMonitor>();
        }

        public static IServiceCollection AddRateMonitor(this IServiceCollection services)
        {
            return services.AddSingleton<IRateMonitor, RateMonitor>();
        }
    }
}
