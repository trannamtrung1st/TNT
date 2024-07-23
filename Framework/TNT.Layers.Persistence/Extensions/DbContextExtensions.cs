using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using TNT.Layers.Domain.Entities;

namespace TNT.Layers.Persistence.Extensions
{
    internal static class DbContextExtensions
    {
        public static EntityEntry<E> SoftRemove<E>(this DbContext dbContext, E entity) where E : class, ISoftDeleteEntity
        {
            entity.IsDeleted = true;
            var entry = dbContext.Entry(entity);
            entry.Property(nameof(entity.IsDeleted)).IsModified = true;
            return entry;
        }

        public static bool TryAttach<T>(this DbContext dbContext, T entity, out EntityEntry<T> entry)
            where T : class
        {
            entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry = dbContext.Attach(entity);
                return true;
            }

            return false;
        }

        public static EntityEntry<E> Update<E>(this DbContext dbContext,
            E entity, params Expression<Func<E, object>>[] changedProperties)
            where E : class
        {
            EntityEntry<E> entry;
            dbContext.TryAttach(entity, out entry);

            if (changedProperties?.Any() == true)
            {
                foreach (var property in changedProperties)
                    entry.Property(property).IsModified = true;
            }
            else return dbContext.Update(entity);

            return entry;
        }
    }
}
