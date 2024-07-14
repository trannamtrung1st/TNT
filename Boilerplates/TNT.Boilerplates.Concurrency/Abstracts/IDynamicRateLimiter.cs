using System;
using TNT.Boilerplates.Concurrency.Models;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface IDynamicRateLimiter : IDisposable
    {
        RateLimiterState State { get; }

        string Name { get; }
        int InitialLimit { get; }
        long ResetLimit();
        long SetLimit(int limit);
        IDisposable Acquire(int count);
        IDisposable TryAcquire(int count);
        void GetRateStatistics(out int availableCountAvg);
        void CollectRate(int movingAverageRange);
    }
}
