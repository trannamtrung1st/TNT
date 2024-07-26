using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using TNT.Layers.Services.AuthHandlers.Client;
using TNT.Layers.Services.Services.Abstracts;

namespace TNT.Layers.Services.Configurations
{
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IOpenApiInfoFactory _openApiInfoFactory;

        public ConfigureSwaggerGenOptions(
            IApiVersionDescriptionProvider provider, IOpenApiInfoFactory openApiInfoFactory)
        {
            _provider = provider;
            _openApiInfoFactory = openApiInfoFactory;
        }

        public virtual void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }

            options.CustomSchemaIds(type => type.FullName.Replace('+', '.'));

            options.EnableAnnotations();

            const string ApplicationApiKey = nameof(ApplicationApiKey);
            const string ApplicationClient = nameof(ApplicationClient);

            options.AddSecurityDefinition(ApplicationApiKey,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter an Authorization Header value",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey,
                });

            options.AddSecurityDefinition(ApplicationClient,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a credentials",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = ClientAuthenticationDefaults.AuthenticationScheme
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApplicationApiKey
                        }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApplicationClient
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }

        public virtual void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        protected virtual OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var apiInfo = _openApiInfoFactory.Create(version: description.GroupName);

            if (description.IsDeprecated)
                apiInfo.Description += $"{Environment.NewLine}This API version has been deprecated.";

            return apiInfo;
        }
    }

}
