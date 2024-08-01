using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace TNT.Layers.Persistence.Abstracts
{
    public interface IDbContext
    {
        IDbContextTransaction CurrentTransaction { get; }
        bool HasActiveTransaction { get; }

        Task ResetState();
        Task<bool> SaveEntities(bool dispatchEvents = true, CancellationToken cancellationToken = default);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task Migrate<TDbContext>(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
        Task SeedMigrations<TDbContext>(IServiceProvider serviceProvider);
        Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    }
}