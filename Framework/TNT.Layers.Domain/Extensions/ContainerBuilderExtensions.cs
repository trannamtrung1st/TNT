using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;
using TNT.Layers.Domain.Abstracts;

namespace TNT.Layers.Domain.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddDomainServices(this ContainerBuilder builder, Assembly[] assemblies, Type[] interceptorTypes)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsClass)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsClass)
                .AssignableTo<IDomainService>()
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}
