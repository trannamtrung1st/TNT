using System;
using System.Collections.Generic;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface ILimiterManager : IDisposable
    {
        IEnumerable<IDynamicRateLimiter> AllLimiters { get; }

        bool TryGetTaskLimiter(string name, out ISyncAsyncTaskLimiter limiter);
        bool TryGetRateLimiter(string name, out IDynamicRateLimiter limiter);
        void AddLimiter(string name, ISyncAsyncTaskLimiter rateLimiter);
        void AddLimiter(string name, IDynamicRateLimiter rateLimiter);
    }
}
