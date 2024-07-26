using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Persistence.QueryFilters.Abstracts;

namespace TNT.Layers.Persistence.QueryFilters
{
    public class NotDeletedQueryFilter : IQueryFilterProvider
    {
        public string ProvideMethodName => nameof(CreateFilter);

        public bool CanApply(IMutableEntityType eType)
            => typeof(ISoftDeleteEntity).IsAssignableFrom(eType.ClrType);

        public Expression<Func<TEntity, bool>> CreateFilter<TEntity>() where TEntity : ISoftDeleteEntity
            => (o) => !o.IsDeleted;
    }
}
