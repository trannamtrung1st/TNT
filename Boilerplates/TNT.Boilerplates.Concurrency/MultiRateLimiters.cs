using TNT.Boilerplates.Concurrency.Abstracts;
using TNT.Boilerplates.Concurrency.Configurations;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Boilerplates.Concurrency
{
    public class MultiRateLimiters : IMultiRateLimiters
    {
        public ISyncAsyncTaskLimiter TaskLimiter { get; }
        public IDynamicRateLimiter SizeLimiter { get; }
        public IEnumerable<IDynamicRateLimiter> RateLimiters { get; }

        public MultiRateLimiters(
            TaskLimiterOptions taskLimiterOptions,
            RateLimiterOptions sizeLimiterOptions,
            ILogger<SyncAsyncTaskLimiter> taskLogger)
        {
            var rateLimiters = new List<IDynamicRateLimiter>();

            if (taskLimiterOptions != null)
                rateLimiters.Add(TaskLimiter = new SyncAsyncTaskLimiter(taskLimiterOptions, taskLogger));

            if (sizeLimiterOptions != null)
                rateLimiters.Add(SizeLimiter = new DynamicRateLimiter(sizeLimiterOptions));

            RateLimiters = rateLimiters;
        }
    }
}