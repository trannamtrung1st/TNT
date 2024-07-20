using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Concurrency.Abstracts;
using TNT.Boilerplates.Concurrency.Configurations;
using System;

namespace TNT.Boilerplates.Concurrency.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryLockManager(this IServiceCollection services)
        {
            return services.AddSingleton<IInMemoryLockManager, InMemoryLockManager>();
        }

        public static IServiceCollection AddDefaultDistributedLockManager(this IServiceCollection services)
        {
            return services.AddSingleton<IDistributedLockManager, InMemoryLockManager>();
        }

        public static IServiceCollection AddSyncAsyncTaskRunner(this IServiceCollection services)
        {
            return services.AddSingleton<ISyncAsyncTaskRunner, SyncAsyncTaskRunner>();
        }

        public static IServiceCollection AddResourceBasedFuzzyRateScaler(this IServiceCollection services)
        {
            return services.AddSingleton<IResourceBasedFuzzyRateScaler, ResourceBasedFuzzyRateScaler>();
        }

        public static IServiceCollection AddResourceBasedRateScaling(this IServiceCollection services, Action<ResourceBasedRateScalingOptions> configure)
        {
            return services.AddSingleton<IRateScalingController, ResourceBasedRateScalingController>()
                .Configure(configure);
        }

        public static IServiceCollection AddLimiterManager(this IServiceCollection services,
            Action<IServiceProvider, ILimiterManager> configure)
        {
            return services.AddSingleton<ILimiterManager>(provider =>
            {
                var manager = new LimiterManager();
                configure(provider, manager);
                return manager;
            });
        }
    }
}
