using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TNT.Boilerplates.AspNetCoreExtensions.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestBuffering(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
        }
    }
}
