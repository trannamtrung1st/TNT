using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using System;
using System.Net;
using System.Net.Http;

namespace TNT.Boilerplates.Resilience
{
    public static class ResilienceBuilder
    {
        public const int DefaultRetries = 3;
        public const int DefaultFirstDelaySeconds = 2;

        public static IAsyncPolicy HandleTransientErrors(
            TimeSpan? medianFirstRetryDelay = null,
            int? retryCount = null,
            Func<Exception, bool> exceptionFilter = null,
            Action<Exception, TimeSpan, int, Context> onRetry = null)
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay ?? TimeSpan.FromSeconds(DefaultFirstDelaySeconds),
                retryCount ?? DefaultRetries);

            AsyncRetryPolicy commonRetry = Policy
                .Handle(exceptionFilter ?? (ex => true))
                .WaitAndRetryAsync(jitterDelay, onRetry: onRetry ?? ((exception, delay, count, context) => { }));

            return commonRetry;
        }

        public static IAsyncPolicy<T> HandleTransientErrors<T>(
            TimeSpan? medianFirstRetryDelay = null,
            int? retryCount = null,
            Func<T, bool> resultFilter = null,
            Func<Exception, bool> exceptionFilter = null,
            Action<DelegateResult<T>, TimeSpan, int, Context> onRetry = null)
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay ?? TimeSpan.FromSeconds(DefaultFirstDelaySeconds),
                retryCount ?? DefaultRetries);

            var httpRetry = Policy<T>
                .HandleResult(resultFilter ?? (result => false))
                .Or(exceptionFilter ?? (ex => true))
                .WaitAndRetryAsync(jitterDelay, onRetry ?? ((result, delay, count, context) => { }));

            return httpRetry;
        }

        public static IAsyncPolicy<HttpResponseMessage> HandleTransientHttpErrors(
            TimeSpan? medianFirstRetryDelay = null,
            int? retryCount = null,
            Func<HttpResponseMessage, bool> resultFilter = null,
            Func<Exception, bool> exceptionFilter = null,
            Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> onRetry = null)
        {
            return HandleTransientErrors(
                medianFirstRetryDelay,
                retryCount,
                resultFilter ?? (resp => (int)resp.StatusCode >= (int)HttpStatusCode.InternalServerError),
                exceptionFilter ?? (ex => ex is HttpRequestException),
                onRetry
            );
        }
    }
}
