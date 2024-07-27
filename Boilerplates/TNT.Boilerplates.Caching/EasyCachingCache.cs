using System;
using System.Threading.Tasks;
using EasyCaching.Core;
using Microsoft.Extensions.Options;
using TNT.Boilerplates.Caching.Abstracts;
using TNT.Boilerplates.Caching.Configurations;

namespace TNT.Boilerplates.Caching
{
    public class EasyCachingCache : ICache
    {
        private readonly IEasyCachingProvider _easyCachingProvider;
        private readonly IOptions<CacheOptions> _options;

        public EasyCachingCache(
            IEasyCachingProvider easyCachingProvider,
            IOptions<CacheOptions> options)
        {
            _easyCachingProvider = easyCachingProvider;
            _options = options;
        }

        private TimeSpan DefaultExpiry => _options.Value.DefaultExpiry;

        public Task<bool> TrySet<T>(string cacheKey, T cacheValue, TimeSpan? expiry = null)
        {
            return _easyCachingProvider.TrySetAsync(cacheKey, cacheValue, expiry ?? DefaultExpiry);
        }

        public async Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiry = null)
        {
            var cacheValue = await _easyCachingProvider.GetAsync(cacheKey, createFunc, expiry ?? DefaultExpiry);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public async Task<T> Get<T>(string cacheKey)
        {
            var cacheValue = await _easyCachingProvider.GetAsync<T>(cacheKey);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public Task Remove(string cacheKey)
        {
            return _easyCachingProvider.RemoveAsync(cacheKey);
        }

        public Task Clear()
        {
            return _easyCachingProvider.FlushAsync();
        }

        public Task<bool> Exists(string cacheKey)
        {
            return _easyCachingProvider.ExistsAsync(cacheKey);
        }
    }
}
