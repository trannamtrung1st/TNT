using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TNT.Layers.Domain.Abstracts;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Persistence.Extensions;

namespace TNT.Layers.Persistence.Repositories
{
    public abstract class EFSoftDeleteRepository<TDbContext, T, TKey> :
        EFDomainRepository<TDbContext, T, TKey>,
        IRepository<T, TKey>
        where T : class, IAggregateRoot, ISoftDeleteEntity
        where TDbContext : DbContext
    {
        protected EFSoftDeleteRepository(TDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task<T> SoftDeleteAsync(T entity)
        {
            entity = dbContext.SoftRemove(entity).Entity;

            return Task.FromResult(entity);
        }
    }
}
