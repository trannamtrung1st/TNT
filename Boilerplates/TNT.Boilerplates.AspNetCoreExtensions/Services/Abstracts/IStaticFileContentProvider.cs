using System.Threading.Tasks;

namespace TNT.Boilerplates.AspNetCoreExtensions.Services.Abstracts
{
    public interface IStaticFileContentProvider
    {
        Task<string> GetIndexContent(string filePath = DefaultWebFiles.Index, bool forceReload = false);
    }
}