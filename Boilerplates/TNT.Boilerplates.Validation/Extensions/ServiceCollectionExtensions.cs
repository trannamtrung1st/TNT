using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Validation.Abstracts;

namespace TNT.Boilerplates.Validation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultSimpleValidatorFactory(this IServiceCollection services)
        {
            return services.AddSingleton<ISimpleValidatorFactory, SimpleValidatorFactory>();
        }

        public static IServiceCollection AddValidation(this IServiceCollection services, Assembly[] assemblies,
            Func<Type, MemberInfo, LambdaExpression, string> displayNameResolver = null)
        {
            displayNameResolver ??= ValidatorOptions.Global.PropertyNameResolver;
            ValidatorOptions.Global.DisplayNameResolver = displayNameResolver;
            return services.AddValidatorsFromAssemblies(assemblies);
        }
    }
}