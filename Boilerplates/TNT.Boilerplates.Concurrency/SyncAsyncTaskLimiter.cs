using TNT.Boilerplates.Concurrency.Abstracts;
using TNT.Boilerplates.Concurrency.Configurations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace TNT.Boilerplates.Concurrency
{
    public class SyncAsyncTaskLimiter : DynamicRateLimiter, ISyncAsyncTaskLimiter
    {
        private int _asyncCount;
        private readonly int _maxAsyncLimit;

        public SyncAsyncTaskLimiter(
            TaskLimiterOptions limiterOptions,
            ILogger<SyncAsyncTaskLimiter> logger) : base(limiterOptions: limiterOptions)
        {
            Options = limiterOptions;
            // Reference: https://engineering.zalando.com/posts/2019/04/how-to-set-an-ideal-thread-pool-size.html
            _maxAsyncLimit = (int)(limiterOptions.AvailableCores * limiterOptions.TargetCpuUtil * (1 + limiterOptions.WaitTime / limiterOptions.ServiceTime));
            logger.LogDebug("Max async limit: {Limit}", _maxAsyncLimit);
        }

        public TaskLimiterOptions Options { get; }

        protected override IDisposable AcquireCore(int count, bool wait)
        {
            var disposable = base.AcquireCore(count, wait);
            if (disposable != null)
                Interlocked.Increment(ref _asyncCount);
            return disposable;
        }

        protected override void Release(int count)
        {
            Interlocked.Decrement(ref _asyncCount);
            base.Release(count);
        }

        protected override bool CanAcquired() => _asyncCount < _maxAsyncLimit && base.CanAcquired();
    }
}