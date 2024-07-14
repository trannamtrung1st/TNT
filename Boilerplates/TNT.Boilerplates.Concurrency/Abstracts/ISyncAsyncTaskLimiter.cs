using TNT.Boilerplates.Concurrency.Configurations;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface ISyncAsyncTaskLimiter : IDynamicRateLimiter
    {
        TaskLimiterOptions Options { get; }
    }
}