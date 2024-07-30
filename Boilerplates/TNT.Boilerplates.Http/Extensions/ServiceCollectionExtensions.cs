using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Http.Handlers;

namespace TNT.Boilerplates.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            return services.AddScoped<ErrorResponseWrapHandler>();
        }
    }
}
