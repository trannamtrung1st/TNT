﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TNT.Boilerplates.Common;

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

        public static IApplicationBuilder UseForwardedHostAsHostHeader(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.Request.Headers.TryGetValue(XHeaderNames.XForwardedHost, out var forwardedHost))
                    context.Request.Headers.Host = forwardedHost;
                await next();
            });
        }

        public static IApplicationBuilder UseHostHeader(this IApplicationBuilder app, string host)
        {
            return app.Use(async (context, next) =>
            {
                context.Request.Headers.Host = host;
                await next();
            });
        }
    }
}
