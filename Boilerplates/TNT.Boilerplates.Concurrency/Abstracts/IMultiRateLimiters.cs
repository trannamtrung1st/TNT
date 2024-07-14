using System.Collections.Generic;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface IMultiRateLimiters
    {
        ISyncAsyncTaskLimiter TaskLimiter { get; }
        IDynamicRateLimiter SizeLimiter { get; }
        IEnumerable<IDynamicRateLimiter> RateLimiters { get; }
    }
}