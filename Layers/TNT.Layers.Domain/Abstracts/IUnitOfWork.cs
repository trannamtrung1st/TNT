using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNT.Layers.Domain.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task ResetStateAsync();

        /// <summary>
        /// [IMPORTANT] Should dispatch domain events and add auditing info
        /// </summary>
        Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default);
    }
}
