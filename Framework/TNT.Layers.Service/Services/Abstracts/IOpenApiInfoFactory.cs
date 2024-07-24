using Microsoft.OpenApi.Models;

namespace TNT.Layers.Service.Services.Abstracts
{
    public interface IOpenApiInfoFactory
    {
        OpenApiInfo Create(string version);
    }
}