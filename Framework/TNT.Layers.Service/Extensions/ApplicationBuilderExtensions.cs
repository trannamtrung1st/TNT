using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using TNT.Layers.Service.Middlewares;

namespace TNT.Layers.Service.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestDataExtraction<TIdentityId>(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestDataExtractionMiddleware<TIdentityId>>();
        }

        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider apiVersionProvider,
            string prefix = SwaggerDefaults.Prefix,
            string endpointFormat = SwaggerDefaults.DocEndpointFormat)
        {
            return app
                .UseSwagger()
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
    }
}
