using System.Threading.Tasks;
using TNT.Layers.Domain.Entities;

namespace TNT.Layers.Domain.Abstracts
{
    public interface IRepository<T, TKey> where T : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> FindByIdAsync(TKey id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
    }

    public interface ISoftDeleteRepository<T, TKey> : IRepository<T, TKey>
        where T : class, IAggregateRoot, ISoftDeleteEntity
    {
        Task<T> SoftDeleteAsync(T entity);
    }
}
