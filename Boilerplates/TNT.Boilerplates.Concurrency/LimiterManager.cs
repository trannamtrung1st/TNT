using System;
using System.Collections.Generic;
using System.Linq;
using TNT.Boilerplates.Concurrency.Abstracts;

namespace TNT.Boilerplates.Concurrency
{
    public class LimiterManager : ILimiterManager, IDisposable
    {
        private readonly Dictionary<string, IDynamicRateLimiter> _limiterMap;
        private readonly Dictionary<string, ISyncAsyncTaskLimiter> _taskLimiterMap;

        public LimiterManager()
        {
            _limiterMap = new Dictionary<string, IDynamicRateLimiter>();
            _taskLimiterMap = new Dictionary<string, ISyncAsyncTaskLimiter>();
        }

        public IEnumerable<IDynamicRateLimiter> AllLimiters => _limiterMap.Values.Concat(_taskLimiterMap.Values);

        public void AddLimiter(string name, ISyncAsyncTaskLimiter rateLimiter)
            => _taskLimiterMap.Add(name, rateLimiter);

        public void AddLimiter(string name, IDynamicRateLimiter rateLimiter)
            => _limiterMap.Add(name, rateLimiter);

        public bool TryGetRateLimiter(string name, out IDynamicRateLimiter limiter)
            => _limiterMap.TryGetValue(name, out limiter);

        public bool TryGetTaskLimiter(string name, out ISyncAsyncTaskLimiter limiter)
            => _taskLimiterMap.TryGetValue(name, out limiter);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (var limiter in _limiterMap.Values)
                limiter.Dispose();
            foreach (var limiter in _taskLimiterMap.Values)
                limiter.Dispose();
        }

    }
}