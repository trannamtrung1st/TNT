using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace TNT.Boilerplates.Common.Mediator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services,
            Assembly[] assemblies, string startsWith = null, Func<Assembly, bool> assemblyFilter = null)
        {
            assemblyFilter ??= assembly => assembly.GetName()?.Name?.StartsWith(startsWith) == true;

            services.Scan(scan => scan.FromApplicationDependencies(assemblyFilter)
                .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                .As(typeof(IPipelineBehavior<,>))
                .WithScopedLifetime());

            return services.AddMediatR(config => config.RegisterServicesFromAssemblies(assemblies));
        }
    }
}
