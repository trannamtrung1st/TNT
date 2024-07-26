using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TNT.Layers.Domain.Abstracts;
using TNT.Layers.Persistence.Services;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Persistence
{
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        protected readonly IMediator mediator;

        public BaseDbContext()
        {
            Utility = new DbContextUtility(this);
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
            Utility = new DbContextUtility(this);
        }

        public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Utility = new DbContextUtility(this, mediator);
        }

        protected virtual DbContextUtility Utility { get; set; }
        public virtual IDbContextTransaction CurrentTransaction => Utility.CurrentTransaction;
        public virtual bool HasActiveTransaction => Utility.HasActiveTransaction;
        private readonly IEnumerable<Assembly> _scanAssemblies = new[] { typeof(DbContextUtility).Assembly };
        public virtual IEnumerable<Assembly> ScanAssemblies => _scanAssemblies;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Utility.OnModelCreating(modelBuilder, scanAssemblies: ScanAssemblies);
        }

        public virtual Task ResetStateAsync() => Utility.ResetStateAsync();

        public virtual async Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default)
            => await Utility.SaveEntitiesAsync(dispatchEvents, cancellationToken);

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

        public virtual Task SeedMigrationsAsync<TDbContext>(IServiceProvider serviceProvider)
            => Utility.SeedMigrationsAsync<TDbContext>(serviceProvider);

        public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => Utility.BeginTransactionAsync(cancellationToken);

        protected virtual void AuditEntities() => Utility.AuditEntities();
    }

    public abstract class BaseDbContext<TIdentityId> : BaseDbContext
    {
        protected readonly IAuthContext<TIdentityId> authContext;

        public BaseDbContext(IAuthContext<TIdentityId> authContext) : base()
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TIdentityId>(this, authContext);
        }

        public BaseDbContext(DbContextOptions options, IAuthContext<TIdentityId> authContext) : base(options)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TIdentityId>(this, authContext);
        }

        public BaseDbContext(DbContextOptions options, IMediator mediator, IAuthContext<TIdentityId> authContext) : base(options, mediator)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TIdentityId>(this, mediator, authContext);
        }
    }
}
