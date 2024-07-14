using System;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface ILockManager
    {
        ILock Acquire(string key, TimeSpan? expiry = null, TimeSpan? timeout = null, int retries = 3);
        void MutexAccess(string key, Action action);
        Task MutexAccess(string key, Func<Task> action);
        T MutexAccess<T>(string key, Func<T> task);
        Task<T> MutexAccess<T>(string key, Func<Task<T>> task);
    }
}
