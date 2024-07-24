using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TNT.Boilerplates.AspNetCoreExtensions.Middlewares
{
    public static class RequestBufferingMiddleware
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
