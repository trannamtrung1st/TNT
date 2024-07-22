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
    }
}