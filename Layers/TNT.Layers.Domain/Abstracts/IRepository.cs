using System.Threading.Tasks;
using TNT.Layers.Domain.Entities;

namespace TNT.Layers.Domain.Abstracts
{
    public interface IRepository<T, TKey> where T : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> FindById(TKey id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }

    public interface ISoftDeleteRepository<T, TKey> : IRepository<T, TKey>
        where T : class, IAggregateRoot, ISoftDeleteEntity
    {
        Task<T> SoftDelete(T entity);
    }
}
