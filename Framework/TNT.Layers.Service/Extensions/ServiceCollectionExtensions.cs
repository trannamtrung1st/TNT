using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using TNT.Layers.Service.Configurations.Options;
using TNT.Layers.Service.Filters;

namespace TNT.Layers.Service.Extensions
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
            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerGenOptions>();
            return services;
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
