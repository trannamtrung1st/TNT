using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace TNT.Boilerplates.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ScanServiceAttributeServices(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            return services.Scan(scan => scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.WithAttribute<TransientServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfTransientServiceAttribute>())
                .AsSelf()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfScopedServiceAttribute>())
                .AsSelf()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfSingletonServiceAttribute>())
                .AsSelf()
                .WithSingletonLifetime());
        }
    }
}
