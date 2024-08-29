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

        public SyncAsyncTaskLimiter(TaskLimiterOptions limiterOptions) : base(limiterOptions: limiterOptions)
        {
            Options = limiterOptions;
        }

        public TaskLimiterOptions Options { get; }

        // Reference: https://engineering.zalando.com/posts/2019/04/how-to-set-an-ideal-thread-pool-size.html
        public int MaxAsyncLimit => (int)(Options.AvailableCores * Options.TargetCpuUtil * (1 + Options.WaitTime / Options.ServiceTime));

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

        protected override bool CanAcquired() => _asyncCount < MaxAsyncLimit && base.CanAcquired();
    }
}