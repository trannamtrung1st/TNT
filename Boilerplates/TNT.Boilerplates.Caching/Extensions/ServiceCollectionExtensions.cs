using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Caching.Abstracts;

namespace TNT.Boilerplates.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMemoryCaches(this IServiceCollection services)
        {
            return services.AddMemoryCache().AddDistributedMemoryCache();
        }

        public static IServiceCollection AddEasyCachingCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICache, EasyCachingCache>();
        }
    }
}