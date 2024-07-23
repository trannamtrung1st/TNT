using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Persistence.Extensions;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Persistence.Services
{
    public class DbContextUtility
    {
        protected readonly IMediator mediator;
        protected readonly DbContext dbContext;

        public DbContextUtility(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbContextUtility(DbContext dbContext, IMediator mediator) : this(dbContext)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public virtual IDbContextTransaction CurrentTransaction => dbContext.Database.CurrentTransaction;
        public virtual bool HasActiveTransaction => dbContext.Database.CurrentTransaction != null;
        private readonly IEnumerable<Assembly> _scanAssemblies = new[] { typeof(DbContextUtility).Assembly };
        public virtual IEnumerable<Assembly> ScanAssemblies => _scanAssemblies;

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var assembly in ScanAssemblies)
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            modelBuilder.RestrictDeleteBehaviour(fkPredicate:
                fk => !fk.GetConstraintName().Contains(ConstraintConstants.NoRestrictForeignKeyConstraintPostfix));

            modelBuilder.AddGlobalQueryFilter(ScanAssemblies);
        }

        public virtual Task ResetStateAsync()
        {
            var entries = dbContext.ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
                entry.State = EntityState.Detached;
            return Task.CompletedTask;
        }

        public virtual async Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default)
        {
            if (dispatchEvents) await mediator.DispatchDomainEventsAsync(dbContext, Domain.DomainEventType.PrePersisted);

            int result = await dbContext.SaveChangesAsync(cancellationToken);

            if (dispatchEvents) await mediator.DispatchDomainEventsAsync(dbContext, Domain.DomainEventType.PostPersisted);

            return true;
        }

        public virtual async Task SeedMigrationsAsync<TDbContext>(IServiceProvider serviceProvider)
        {
            if (this.dbContext is not TDbContext targetDbContext)
                return;

            using var transaction = await BeginTransactionAsync();

            foreach (var task in MigrationTasks<TDbContext>.Tasks)
                await task(targetDbContext, serviceProvider);

            MigrationTasks<TDbContext>.Tasks.Clear();

            await transaction.CommitAsync();
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction) return null;

            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public virtual void AuditEntities()
        {
            var hasChanges = dbContext.ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = dbContext.ChangeTracker.Entries()
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

    public class DbContextUtility<TIdentityId> : DbContextUtility
    {
        protected readonly IAuthContext<TIdentityId> authContext;

        public DbContextUtility(DbContext dbContext, IAuthContext<TIdentityId> authContext) : base(dbContext)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public DbContextUtility(DbContext dbContext, IMediator mediator, IAuthContext<TIdentityId> authContext) : base(dbContext, mediator)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public override void AuditEntities()
        {
            var hasChanges = dbContext.ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = dbContext.ChangeTracker.Entries()
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