using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNT.Layers.Domain.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task ResetState();

        /// <summary>
        /// [IMPORTANT] Should dispatch domain events and add auditing info
        /// </summary>
        Task<bool> SaveEntities(bool dispatchEvents = true, CancellationToken cancellationToken = default);
    }
}
