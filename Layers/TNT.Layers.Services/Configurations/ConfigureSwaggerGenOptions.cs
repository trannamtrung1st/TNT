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
        private readonly IOptions<OpenIdInfo> _openIdInfo;

        public ConfigureSwaggerGenOptions(
            IApiVersionDescriptionProvider provider,
            IOpenApiInfoFactory openApiInfoFactory,
            IOptions<OpenIdInfo> openIdInfo)
        {
            _provider = provider;
            _openApiInfoFactory = openApiInfoFactory;
            _openIdInfo = openIdInfo;
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

            var requirement = new OpenApiSecurityRequirement();
            options.AddSecurityRequirement(requirement);

            ConfigureApiKeyScheme(options, requirement);

            ConfigureClientScheme(options, requirement);

            if (_openIdInfo.Value.PublicConfigurationEndpoint is not null)
                ConfigureOpenIdScheme(options, requirement);
        }

        public virtual void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        protected virtual void ConfigureApiKeyScheme(SwaggerGenOptions options, OpenApiSecurityRequirement requirement)
        {
            const string ApplicationApiKey = nameof(ApplicationApiKey);

            options.AddSecurityDefinition(ApplicationApiKey,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter an Authorization Header value",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey,
                });

            requirement[new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApplicationApiKey
                }
            }] = Array.Empty<string>();
        }

        protected virtual void ConfigureClientScheme(SwaggerGenOptions options, OpenApiSecurityRequirement requirement)
        {
            const string ApplicationClient = nameof(ApplicationClient);

            options.AddSecurityDefinition(ApplicationClient,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a credentials",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = ClientAuthenticationDefaults.AuthenticationScheme
                });

            requirement[new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApplicationClient
                }
            }] = Array.Empty<string>();
        }

        protected virtual void ConfigureOpenIdScheme(SwaggerGenOptions options, OpenApiSecurityRequirement requirement)
        {
            const string ApplicationOpenId = nameof(ApplicationOpenId);

            var openIdUrl = new Uri(
                baseUri: new Uri(_openIdInfo.Value.IdpPublicUrl),
                relativeUri: _openIdInfo.Value.PublicConfigurationEndpoint);
            options.AddSecurityDefinition(ApplicationOpenId,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = openIdUrl
                });

            requirement[new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApplicationOpenId
                }
            }] = Array.Empty<string>();
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
