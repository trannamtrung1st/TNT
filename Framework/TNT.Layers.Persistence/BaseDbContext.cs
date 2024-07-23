using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TNT.Layers.Domain.Abstracts;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Persistence.Extensions;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Persistence
{
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        protected readonly IMediator mediator;

        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public virtual IDbContextTransaction CurrentTransaction => Database.CurrentTransaction;
        public virtual bool HasActiveTransaction => Database.CurrentTransaction != null;
        private readonly IEnumerable<Assembly> _scanAssemblies = new[] { typeof(BaseDbContext).Assembly };
        protected virtual IEnumerable<Assembly> ScanAssemblies => _scanAssemblies;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var assembly in ScanAssemblies)
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            modelBuilder.RestrictDeleteBehaviour(fkPredicate:
                fk => !fk.GetConstraintName().Contains(ConstraintConstants.NoRestrictForeignKeyConstraintPostfix));

            modelBuilder.AddGlobalQueryFilter(ScanAssemblies);
        }

        public virtual Task ResetStateAsync()
        {
            var entries = ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
            return Task.CompletedTask;
        }

        public virtual async Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default)
        {
            if (dispatchEvents) await mediator.DispatchDomainEventsAsync(this, Domain.DomainEventType.PrePersisted);

            int result = await SaveChangesAsync(cancellationToken);

            if (dispatchEvents) await mediator.DispatchDomainEventsAsync(this, Domain.DomainEventType.PostPersisted);

            return true;
        }

        #region SaveChanges
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion

        public virtual async Task SeedMigrationsAsync<TDbContext>(IServiceProvider serviceProvider)
        {
            if (this is not TDbContext dbContext)
                return;

            using var transaction = await BeginTransactionAsync();

            foreach (var task in MigrationTasks<TDbContext>.Tasks)
                await task(dbContext, serviceProvider);

            MigrationTasks<TDbContext>.Tasks.Clear();

            await transaction.CommitAsync();
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction) return null;

            return await Database.BeginTransactionAsync(cancellationToken);
        }

        protected virtual void AuditEntities()
        {
            var hasChanges = ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = ChangeTracker.Entries()
                .Where(o => o.State == EntityState.Modified ||
                    o.State == EntityState.Added).ToArray();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            var isSoftDeleted = false;

                            if (entity is ISoftDeleteEntity softDeleteEntity)
                            {
                                isSoftDeleted = entry.Property(nameof(ISoftDeleteEntity.IsDeleted)).IsModified
                                    && softDeleteEntity.IsDeleted;

                                if (isSoftDeleted)
                                {
                                    softDeleteEntity.DeletedTime = DateTimeOffset.UtcNow;
                                    isSoftDeleted = true;
                                }
                            }

                            if (!isSoftDeleted && entity is IAuditableEntity auditableEntity)
                                auditableEntity.LastModifiedTime = DateTimeOffset.UtcNow;
                            break;
                        }
                    case EntityState.Added:
                        {
                            if (entity is IAuditableEntity auditableEntity)
                                auditableEntity.CreatedTime = DateTimeOffset.UtcNow;
                            break;
                        }
                }
            }
        }
    }

    public abstract class BaseDbContext<TIdentityId> : BaseDbContext
    {
        protected readonly IAuthContext<TIdentityId> authContext;

        public BaseDbContext(IAuthContext<TIdentityId> authContext) : base()
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public BaseDbContext(DbContextOptions options, IAuthContext<TIdentityId> authContext) : base(options)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public BaseDbContext(DbContextOptions options, IMediator mediator, IAuthContext<TIdentityId> authContext) : base(options, mediator)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        protected override void AuditEntities()
        {
            var hasChanges = ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = ChangeTracker.Entries()
                .Where(o => o.State == EntityState.Modified ||
                    o.State == EntityState.Added).ToArray();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            var isSoftDeleted = false;

                            if (entity is ISoftDeleteEntity softDeleteEntity)
                            {
                                isSoftDeleted = entry.Property(nameof(ISoftDeleteEntity.IsDeleted)).IsModified
                                    && softDeleteEntity.IsDeleted;

                                if (isSoftDeleted)
                                {
                                    softDeleteEntity.DeletedTime = DateTimeOffset.UtcNow;
                                    isSoftDeleted = true;

                                    if (entity is ISoftDeleteEntity<TIdentityId> userSoftDeleteEntity)
                                        userSoftDeleteEntity.DeletorId = authContext.IdentityId;
                                }
                            }

                            if (!isSoftDeleted && entity is IAuditableEntity auditableEntity)
                            {
                                auditableEntity.LastModifiedTime = DateTimeOffset.UtcNow;

                                if (!isSoftDeleted && entity is IAuditableEntity<TIdentityId> userAuditableEntity)
                                    userAuditableEntity.LastModifyUserId = authContext.IdentityId;
                            }
                            break;
                        }
                    case EntityState.Added:
                        {
                            if (entity is IAuditableEntity auditableEntity)
                            {
                                auditableEntity.CreatedTime = DateTimeOffset.UtcNow;

                                if (entity is IAuditableEntity<TIdentityId> userAuditableEntity)
                                    userAuditableEntity.CreatorId = authContext.IdentityId;
                            }
                            break;
                        }
                }
            }
        }
    }
}
