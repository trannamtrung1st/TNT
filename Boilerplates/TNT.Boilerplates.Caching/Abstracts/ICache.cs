using System;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Caching.Abstracts
{
    public interface ICache
    {
        Task<bool> TrySet<T>(string cacheKey, T cacheValue, TimeSpan? expiry = null);
        Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiry = null);
        Task<T> Get<T>(string cacheKey);
        Task Remove(string cacheKey);
        Task<bool> Exists(string cacheKey);
        Task Clear();
    }
}
