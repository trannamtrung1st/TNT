using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace TNT.Boilerplates.Common.DependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddServiceAttributeServices(this ContainerBuilder builder, Assembly[] assemblies, Type[] interceptorTypes)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfTransientServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfScopedServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfSingletonServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(TransientServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(ScopedServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SingletonServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .SingleInstance();

            return builder;
        }
    }
}
