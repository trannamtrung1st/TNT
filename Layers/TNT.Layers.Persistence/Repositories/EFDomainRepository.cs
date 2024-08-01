using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;
using TNT.Layers.Domain.Abstracts;
using TNT.Layers.Domain.Entities;

namespace TNT.Layers.Persistence.Repositories
{
    public abstract class EFDomainRepository<TDbContext, T, TKey> :
        IRepository<T, TKey>
        where T : class, IAggregateRoot
        where TDbContext : DbContext
    {
        private static readonly EntityState[] CanUpdateStates = new[]
        {
            EntityState.Detached,
            EntityState.Unchanged,
            EntityState.Modified
        };
        protected readonly TDbContext dbContext;
        protected readonly IUnitOfWork unitOfWork;

        public EFDomainRepository(TDbContext dbContext, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => unitOfWork;
        public IQueryable<T> NoTrackedQuery => dbContext.Set<T>().AsNoTracking();
        public IQueryable<T> TrackedQuery => dbContext.Set<T>().AsTracking();

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                return (await dbContext.AddAsync(entity)).Entity;
            }

            return entity;
        }

        public Task<T> Update(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (CanUpdateStates.Contains(entry.State))
            {
                return Task.FromResult(dbContext.Update(entity).Entity);
            }

            return Task.FromResult(entity);
        }

        public Task<T> Delete(T entity)
        {
            entity = dbContext.Remove(entity).Entity;

            return Task.FromResult(entity);
        }

        public abstract Task<T> FindById(TKey id);
    }
}
