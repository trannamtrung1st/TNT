using Microsoft.OpenApi.Models;

namespace TNT.Layers.Services.Services.Abstracts
{
    public interface IOpenApiInfoFactory
    {
        OpenApiInfo Create(string version);
    }
}