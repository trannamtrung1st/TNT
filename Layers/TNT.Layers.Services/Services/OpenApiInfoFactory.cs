using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TNT.Layers.Services.Configurations;
using TNT.Layers.Services.Services.Abstracts;

namespace TNT.Layers.Services.Services
{
    public class OpenApiInfoFactory : IOpenApiInfoFactory
    {
        private readonly IOptions<OpenApiInfoOptions> _options;

        public OpenApiInfoFactory(IOptions<OpenApiInfoOptions> options)
        {
            _options = options;
        }

        public OpenApiInfo Create(string version)
            => new OpenApiInfo
            {
                Title = _options.Value.Title,
                Description = _options.Value.Description,
                Version = version
            };
    }
}
