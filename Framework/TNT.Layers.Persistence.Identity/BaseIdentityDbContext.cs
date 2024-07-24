using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

namespace TNT.Layers.Persistence.Identity
{
    // [NOTE] same implementation as BaseDbContext
    public abstract class BaseIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IUnitOfWork
        where TUser : IdentityUser<TKey> where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey> where TUserClaim : IdentityUserClaim<TKey> where TUserRole : IdentityUserRole<TKey> where TUserLogin : IdentityUserLogin<TKey> where TRoleClaim : IdentityRoleClaim<TKey> where TUserToken : IdentityUserToken<TKey>
    {
        protected readonly IMediator mediator;

        public BaseIdentityDbContext()
        {
            Utility = new DbContextUtility(this);
        }

        public BaseIdentityDbContext(DbContextOptions options) : base(options)
        {
            Utility = new DbContextUtility(this);
        }

        public BaseIdentityDbContext(DbContextOptions options, IMediator mediator) : base(options)
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

    public abstract class BaseIdentityDbContext<TAppIdentityId, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : BaseIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IUnitOfWork
        where TUser : IdentityUser<TKey> where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey> where TUserClaim : IdentityUserClaim<TKey> where TUserRole : IdentityUserRole<TKey> where TUserLogin : IdentityUserLogin<TKey> where TRoleClaim : IdentityRoleClaim<TKey> where TUserToken : IdentityUserToken<TKey>
    {
        protected readonly IAuthContext<TAppIdentityId> authContext;

        public BaseIdentityDbContext(IAuthContext<TAppIdentityId> authContext) : base()
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TAppIdentityId>(this, authContext);
        }

        public BaseIdentityDbContext(DbContextOptions options, IAuthContext<TAppIdentityId> authContext) : base(options)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TAppIdentityId>(this, authContext);
        }

        public BaseIdentityDbContext(DbContextOptions options, IMediator mediator, IAuthContext<TAppIdentityId> authContext) : base(options, mediator)
        {
            this.authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
            Utility = new DbContextUtility<TAppIdentityId>(this, mediator, authContext);
        }
    }

    public abstract class BaseIdentityDbContext<TAppIdentityId, TUser, TKey, TRole>
        : BaseIdentityDbContext<TAppIdentityId, TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TUser : IdentityUser<TKey> where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        protected BaseIdentityDbContext(IAuthContext<TAppIdentityId> authContext) : base(authContext)
        {
        }

        protected BaseIdentityDbContext(DbContextOptions options, IAuthContext<TAppIdentityId> authContext) : base(options, authContext)
        {
        }

        protected BaseIdentityDbContext(DbContextOptions options, IMediator mediator, IAuthContext<TAppIdentityId> authContext) : base(options, mediator, authContext)
        {
        }
    }
}
