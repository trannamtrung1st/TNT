using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Concurrency.Abstracts;
using TNT.Boilerplates.Concurrency.Configurations;
using Microsoft.Extensions.Logging;
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

        public static IServiceCollection AddMultiRateLimiters(this IServiceCollection services,
            Action<TaskLimiterOptions> configureTaskLimiter = null,
            Action<RateLimiterOptions> configureSizeLimiter = null)
        {
            return services.AddSingleton<IMultiRateLimiters>(provider =>
            {
                var taskLogger = provider.GetRequiredService<ILogger<SyncAsyncTaskLimiter>>();
                TaskLimiterOptions taskLimiterOptions = null;
                RateLimiterOptions sizeLimiterOptions = null;

                if (configureTaskLimiter != null)
                {
                    taskLimiterOptions = new TaskLimiterOptions();
                    configureTaskLimiter(taskLimiterOptions);
                }

                if (configureSizeLimiter != null)
                {
                    sizeLimiterOptions = new RateLimiterOptions();
                    configureSizeLimiter(sizeLimiterOptions);
                }

                return new MultiRateLimiters(
                    taskLimiterOptions,
                    sizeLimiterOptions,
                    taskLogger
                );
            });
        }
    }
}
