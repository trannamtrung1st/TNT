﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TNT.Boilerplates.Common.Reflection;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Persistence.QueryFilters.Abstracts;

namespace TNT.Layers.Persistence.Extensions
{
    public static class EntityConfigExtensions
    {
        public const string DefaultFkPrefix = "FK";

        public static EntityTypeBuilder<T> ConfigureAuditableEntityWithStringKey<T>(this EntityTypeBuilder<T> builder,
            int? userKeyStringLength = null,
            bool isUnicode = false) where T : class
        {
            var entityType = typeof(T);

            if (typeof(IAuditableEntity<string>).IsAssignableFrom(entityType))
            {
                var createdUserId = builder.Property(o => (o as IAuditableEntity<string>).CreatedTime)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    createdUserId.HasMaxLength(userKeyStringLength.Value);

                var lastModifiedUserId = builder.Property(o => (o as IAuditableEntity<string>).LastModifyUserId)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    lastModifiedUserId.HasMaxLength(userKeyStringLength.Value);
            }

            if (typeof(ISoftDeleteEntity<string>).IsAssignableFrom(entityType))
            {
                var deletedUserId = builder.Property(o => (o as ISoftDeleteEntity<string>).DeletorId)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    deletedUserId.HasMaxLength(userKeyStringLength.Value);
            }

            return builder;
        }

        public static ModelBuilder RestrictDeleteBehaviour(this ModelBuilder builder,
            Func<IMutableEntityType, bool> typePredicate = null,
            Func<IMutableForeignKey, bool> fkPredicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (typePredicate != null)
                entityTypes = entityTypes.Where(typePredicate);

            var foreignKeys = entityTypes.SelectMany(e => e.GetForeignKeys());

            if (fkPredicate != null)
                foreignKeys = foreignKeys.Where(fkPredicate);

            foreach (var foreignKey in foreignKeys)
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            return builder;
        }

        public static ModelBuilder AddGlobalQueryFilter(this ModelBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            var filterProviders = ReflectionHelper.GetAllTypesAssignableTo(
                typeof(IQueryFilterProvider), assemblies).Select(o => o.CreateInstance<IQueryFilterProvider>())
                    .ToArray();

            if (!filterProviders.Any()) return builder;

            var eTypes = builder.Model.GetEntityTypes();

            foreach (var entityType in eTypes)
            {
                // skip Shadow types
                if (entityType.ClrType == null) continue;

                LambdaExpression finalExpr = null;

                foreach (var provider in filterProviders)
                {
                    LambdaExpression andExpr = null;

                    if (provider.CanApply(entityType))
                    {
                        andExpr = provider.GetType().GetInstanceMethod(provider.ProvideMethodName)
                            .InvokeGeneric<LambdaExpression>(provider, new[] { entityType.ClrType });
                    }

                    if (andExpr == null) continue;

                    if (finalExpr == null) finalExpr = andExpr;
                    else finalExpr = finalExpr.And(andExpr);
                }

                if (finalExpr != null)
                    entityType.SetQueryFilter(finalExpr);
            }

            return builder;
        }

        public static ReferenceCollectionBuilder<Sub, Ref> HasDefaultConstraintName<Sub, Ref>(
            this ReferenceCollectionBuilder<Sub, Ref> builder,
            string postfix = null)
            where Sub : class
            where Ref : class
        {
            string name = $"{builder.Metadata.GetDefaultName()}{postfix ?? string.Empty}";

            return builder.HasConstraintName(name);
        }

        public static ReferenceCollectionBuilder<Sub, Ref> HasDefaultConstraintName<Sub, Ref>(
            this ReferenceCollectionBuilder<Sub, Ref> builder,
            string prefix = DefaultFkPrefix,
            string principal = null, string dependent = null, string key = null,
            string postfix = null)
            where Sub : class
            where Ref : class
        {
            return builder.HasConstraintName(
                builder.Metadata.GetDefaultConstraintName(prefix, principal, dependent, key, postfix));
        }

        private static string GetDefaultConstraintName(this IMutableForeignKey metadata,
            string prefix = DefaultFkPrefix,
            string principal = null, string dependent = null, string key = null, string postfix = null)
        {
            principal ??= metadata.PrincipalEntityType.Name;
            dependent ??= metadata.DeclaringEntityType.Name;
            key ??= string.Join('_', metadata.Properties.Select(prop => prop.Name));

            return $"{prefix}_{dependent}_{principal}_{key}{postfix ?? string.Empty}";
        }

        public static ModelBuilder UseEntityTypeNameForTable(this ModelBuilder builder,
            Func<IMutableEntityType, bool> predicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (predicate != null)
                entityTypes = entityTypes.Where(predicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                    entityType.SetTableName(entityType.ClrType.Name);
            }

            return builder;
        }

        public static ModelBuilder AdjustTableName(this ModelBuilder builder,
            Func<IMutableEntityType, string> func,
            Func<IMutableEntityType, bool> entityTypePredicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (entityTypePredicate != null)
                entityTypes = entityTypes.Where(entityTypePredicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                    entityType.SetTableName(func(entityType));
            }

            return builder;
        }

        public static ModelBuilder RestrictStringLength(this ModelBuilder builder,
            int maxLength, bool? setIsFixedLength = null,
            bool unboundNormalColumnsOnly = true,
            Func<IMutableProperty, bool> extraColumnPredicate = null,
            Func<IMutableEntityType, bool> entityTypePredicate = null,
            IEnumerable<string> testColumnTypes = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (entityTypePredicate != null)
                entityTypes = entityTypes.Where(entityTypePredicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                {
                    var strProps = entityType.GetProperties()
                        .Where(o => o.ClrType == typeof(string)
                            && testColumnTypes?.Contains(o.GetColumnType()) != true);

                    if (unboundNormalColumnsOnly)
                        strProps = strProps.Where(IsUnboundLength)
                            .Where(o => !o.IsForeignKey());

                    if (extraColumnPredicate != null)
                        strProps = strProps.Where(extraColumnPredicate);

                    foreach (var prop in strProps)
                    {
                        prop.SetMaxLength(maxLength);

                        if (setIsFixedLength != null)
                            prop.SetIsFixedLength(setIsFixedLength);
                    }
                }
            }

            return builder;
        }

        // include string foreign keys (also unbound length)
        public static bool IsUnboundLength(this IMutableProperty prop)
        {
            return prop.GetMaxLength() == null;
        }

        public static bool IsSoftDeleteEntity(this Type type)
        {
            return typeof(ISoftDeleteEntity).IsAssignableFrom(type);
        }

        public static bool IsAnyPropertyOfTypes(this IMutableProperty prop, IEnumerable<Type> types)
        {
            var entityType = prop.DeclaringType.ClrType;

            return entityType == null ||
                types.Any(t => t.IsAssignableFrom(entityType));
        }
    }
}
