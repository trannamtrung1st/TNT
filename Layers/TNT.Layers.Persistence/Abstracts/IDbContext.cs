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

        Task ResetStateAsync();
        Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task MigrateAsync<TDbContext>(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
        Task SeedMigrationsAsync<TDbContext>(IServiceProvider serviceProvider);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}