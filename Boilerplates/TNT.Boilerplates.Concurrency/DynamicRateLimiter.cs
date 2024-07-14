using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TNT.Boilerplates.Common.Disposable;
using TNT.Boilerplates.Concurrency.Abstracts;
using TNT.Boilerplates.Concurrency.Configurations;
using TNT.Boilerplates.Concurrency.Models;

namespace TNT.Boilerplates.Concurrency
{
    public class DynamicRateLimiter : IDynamicRateLimiter, IDisposable
    {
        private readonly ManualResetEventSlim _availableEvent;
        private readonly object _lock = new object();
        private readonly RateLimiterOptions _limiterOptions;
        private readonly Queue<long> _availableCounts = new Queue<long>();
        private int _limit = 0;
        private int _acquired = 0;

        public DynamicRateLimiter(RateLimiterOptions limiterOptions)
        {
            _availableEvent = new ManualResetEventSlim();
            _limiterOptions = limiterOptions;
            SetLimit(limit: _limiterOptions.InitialLimit);
        }

        private int Available
        {
            get
            {
                lock (_lock) { return _limit - _acquired; }
            }
        }
        public string Name => _limiterOptions.Name;
        public int InitialLimit => _limiterOptions.InitialLimit;
        public RateLimiterState State
        {
            get
            {
                lock (_lock) { return new RateLimiterState(_limit, _acquired, _limit - _acquired); }
            }
        }

        public IDisposable Acquire(int count)
            => AcquireCore(count, wait: true);

        public IDisposable TryAcquire(int count)
            => AcquireCore(count, wait: false);

        protected virtual void Release(int count)
        {
            lock (_lock)
            {
                if (_acquired > 0)
                {
                    _acquired -= count;
                    _availableEvent.Set();
                }
            }
        }

        public long SetLimit(int limit)
        {
            var acceptedLimit = GetAcceptedLimit(limit);
            lock (_lock)
            {
                var prevLimit = _limit;
                _limit = acceptedLimit;
                if (_limit > prevLimit) _availableEvent.Set();
            }
            return acceptedLimit;
        }

        public long ResetLimit() => SetLimit(limit: _limiterOptions.InitialLimit);

        protected virtual IDisposable AcquireCore(int count, bool wait)
        {
            if (_limit == 0)
                return null;
            bool acquired = false;
            while (!acquired)
            {
                lock (_lock)
                {
                    if (CanAcquired())
                    {
                        acquired = true;
                        _acquired += count;
                    }
                    else _availableEvent.Reset();
                }

                if (!acquired)
                {
                    if (wait) _availableEvent.Wait();
                    else return null;
                }
            }
            return new SimpleScope(() => Release(count));
        }

        protected virtual int GetAcceptedLimit(int limit) => limit;

        protected virtual bool CanAcquired() => _acquired < _limit;

        public void GetRateStatistics(out int availableCountAvg)
        {
            availableCountAvg = _availableCounts.Count > 0 ? (int)_availableCounts.Average() : 0;
        }

        public void CollectRate(int movingAverageRange)
        {
            if (_availableCounts.Count == movingAverageRange) _availableCounts.TryDequeue(out var _);
            _availableCounts.Enqueue(Available);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            _availableEvent.Dispose();
        }
    }
}