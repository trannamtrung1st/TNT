using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using TNT.Layers.Services.Configurations;
using TNT.Layers.Services.Filters;
using TNT.Layers.Services.Middlewares;
using TNT.Layers.Services.Services;
using TNT.Layers.Services.Services.Abstracts;

namespace TNT.Layers.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // [NOTE] client SDK services, resilience

        public static IServiceCollection AddApiVersioningDefaults(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            })
                .AddApiExplorer(opt =>
                {
                    opt.GroupNameFormat = ApiDefaults.VersionGroupNameFormat;
                });

            return services;
        }

        public static IServiceCollection AddSwaggerDefaults(this IServiceCollection services)
        {
            return services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .ConfigureOptions<ConfigureSwaggerGenOptions>();
        }

        public static IServiceCollection ConfigureApiBehaviorDefaults(this IServiceCollection services,
            Action<ApiBehaviorOptions> extraConfigure = null)
        {
            return services.Configure<ApiBehaviorOptions>(opt =>
            {
                // [IMPORTANT] Disable automatic 400 response
                opt.SuppressModelStateInvalidFilter = true;
                extraConfigure?.Invoke(opt);
            });
        }

        public static IServiceCollection AddOpenApiInfoFactory(this IServiceCollection services,
            Action<OpenApiInfoOptions> configure = null)
        {
            return services
                .AddSingleton<IOpenApiInfoFactory, OpenApiInfoFactory>()
                .Configure<OpenApiInfoOptions>(opt => configure?.Invoke(opt));
        }

        public static IServiceCollection AddRequestDataExtraction<TIdentityId>(this IServiceCollection services)
        {
            return services.AddScoped<RequestDataExtractionMiddleware<TIdentityId>>();
        }

        public static IMvcBuilder AddControllersDefaults(this IServiceCollection services,
            Action<MvcOptions> extraConfigure = null)
        {
            return services.AddControllers(opt =>
            {
                opt.Filters.Add<ValidateModelStateFilter>();
                opt.Filters.Add<ApiExceptionFilter>();
                opt.Filters.Add<ApiResponseWrapFilter>();
                extraConfigure?.Invoke(opt);
            });
        }
    }
}
