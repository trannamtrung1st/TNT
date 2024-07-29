using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TNT.Layers.Persistence.Abstracts;
using TNT.Layers.Services.Middlewares;

namespace TNT.Layers.Services.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestDataExtraction<TIdentityId>(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestDataExtractionMiddleware<TIdentityId>>();
        }

        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider apiVersionProvider,
            string routeTemplate = SwaggerDefaults.RouteTemplate,
            string prefix = SwaggerDefaults.Prefix,
            string endpointFormat = SwaggerDefaults.DocEndpointFormat)
        {
            return app
                .UseSwagger(options => options.RouteTemplate = routeTemplate)
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = prefix;
                    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                    {
                        var versionStr = description.GroupName;
                        options.SwaggerEndpoint(
                            string.Format(endpointFormat, versionStr),
                            versionStr);
                    }
                });
        }

        public static async Task<IApplicationBuilder> Migrate<TDbContext>(this IApplicationBuilder app, CancellationToken cancellationToken = default)
            where TDbContext : IDbContext
        {
            var provider = app.ApplicationServices;
            var dbContext = provider.GetRequiredService<TDbContext>();
            await dbContext.MigrateAsync<TDbContext>(provider, cancellationToken);
            return app;
        }
    }
}
