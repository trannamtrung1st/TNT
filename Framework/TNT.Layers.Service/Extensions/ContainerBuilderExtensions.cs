using Autofac;
using System;
using System.Reflection;
using TNT.Boilerplates.Common.DependencyInjection;
using TNT.Layers.Domain.Extensions;

namespace TNT.Layers.Service.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddServicesFromLayers(this ContainerBuilder builder, Assembly[] assemblies, Type[] interceptorTypes = null)
        {
            interceptorTypes ??= InterceptorDefaults.Types;

            builder.AddDomainServices(assemblies, interceptorTypes);

            builder.AddServiceAttributeServices(assemblies, interceptorTypes);

            return builder;
        }
    }
}
