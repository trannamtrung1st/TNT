using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TNT.Layers.Persistence.Abstracts;
using TNT.Layers.Services.Configurations;
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

        public static IApplicationBuilder UseSimpleRequestLogging(
            this IApplicationBuilder app, string configKey = SimpleRequestLoggingOptions.DefaultConfigKey)
        {
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var options = configuration.GetSection(configKey).Get<SimpleRequestLoggingOptions>();
            return app.UseSimpleRequestLogging(options);
        }

        public static IApplicationBuilder UseSimpleRequestLogging(
            this IApplicationBuilder app, SimpleRequestLoggingOptions simpleOptions)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                simpleOptions.CopyTo(options);
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var context = httpContext.Features.Get<IExceptionHandlerFeature>();
                    var exception = context?.Error;
                    diagnosticContext.SetException(exception);
                    diagnosticContext.Set(nameof(Environment.NewLine), Environment.NewLine);

                    foreach (var header in simpleOptions.EnrichHeaders)
                        diagnosticContext.Set(header.Key, httpContext.Request.Headers[header.Value]);
                };
            });
        }

        public static async Task<IApplicationBuilder> Migrate<TDbContext>(this IApplicationBuilder app, CancellationToken cancellationToken = default)
            where TDbContext : IDbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var provider = scope.ServiceProvider;
            var dbContext = provider.GetRequiredService<TDbContext>();
            await dbContext.MigrateAsync<TDbContext>(provider, cancellationToken);
            return app;
        }
    }
}
