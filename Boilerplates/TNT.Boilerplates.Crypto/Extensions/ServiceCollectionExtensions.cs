using Microsoft.Extensions.DependencyInjection;
using TNT.Boilerplates.Crypto.Abstracts;

namespace TNT.Boilerplates.Crypto.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {
            return services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
