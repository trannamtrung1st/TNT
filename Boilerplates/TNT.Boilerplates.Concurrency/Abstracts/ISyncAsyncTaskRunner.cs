using System;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface ISyncAsyncTaskRunner
    {
        Task RunSyncAsync(IDisposable asyncScope, Func<IAsyncDisposable, Task> task, bool longRunning = true);
        Task RunSyncAsync(IAsyncDisposable asyncScope, Func<IAsyncDisposable, Task> task, bool longRunning = true);
    }
}