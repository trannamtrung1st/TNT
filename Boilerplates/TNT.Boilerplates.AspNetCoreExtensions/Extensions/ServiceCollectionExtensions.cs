using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.AspNetCoreExtensions.Services;
using TNT.Boilerplates.AspNetCoreExtensions.Services.Abstracts;

namespace TNT.Boilerplates.AspNetCoreExtensions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebRootStaticFileContentProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IStaticFileContentProvider, WebRootStaticFileContentProvider>();
        }
    }
}