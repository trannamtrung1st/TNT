using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using System;

namespace TNT.Boilerplates.Resilience
{
    public static class ResilienceBuilder
    {
        public const int DefaultRetries = 3;
        public const int DefaultFirstDelaySeconds = 2;

        public static IAsyncPolicy HandleTransientErrors(
            TimeSpan? medianFirstRetryDelay = default,
            int retryCount = DefaultRetries,
            Func<Exception, bool> exceptionFilter = null,
            Action<Exception, TimeSpan, int, Context> onRetry = null)
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay ?? TimeSpan.FromSeconds(DefaultFirstDelaySeconds),
                retryCount);

            AsyncRetryPolicy commonRetry = Policy
                .Handle(exceptionFilter ?? (ex => true))
                .WaitAndRetryAsync(jitterDelay, onRetry: onRetry ?? ((exception, delay, count, context) => { }));

            return commonRetry;
        }

        public static IAsyncPolicy<T> HandleTransientErrors<T>(
            TimeSpan? medianFirstRetryDelay = default,
            int retryCount = DefaultRetries,
            Func<T, bool> resultFilter = null,
            Func<Exception, bool> exceptionFilter = null,
            Action<DelegateResult<T>, TimeSpan, int, Context> onRetry = null)
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay ?? TimeSpan.FromSeconds(DefaultFirstDelaySeconds),
                retryCount);

            var httpRetry = Policy<T>
                .HandleResult(resultFilter ?? (result => false))
                .Or(exceptionFilter ?? (ex => true))
                .WaitAndRetryAsync(jitterDelay, onRetry ?? ((result, delay, count, context) => { }));

            return httpRetry;
        }
    }
}
