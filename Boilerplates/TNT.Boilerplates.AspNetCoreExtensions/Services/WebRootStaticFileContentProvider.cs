using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TNT.Boilerplates.AspNetCoreExtensions.Services.Abstracts;
using System.IO;

namespace TNT.Boilerplates.AspNetCoreExtensions.Services
{
    public class WebRootStaticFileContentProvider : IStaticFileContentProvider
    {
        private readonly IWebHostEnvironment _env;

        public WebRootStaticFileContentProvider(IWebHostEnvironment env)
        {
            _env = env;
        }

        private static string _cacheIndexContent;
        public async Task<string> GetIndexContent(string filePath = "index.html", bool forceReload = false)
        {
            if (_cacheIndexContent == null || forceReload)
            {
                var fileInfo = _env.WebRootFileProvider.GetFileInfo(filePath);
                using var stream = fileInfo.CreateReadStream();
                _cacheIndexContent = await stream.ReadAsStringAsync();
            }
            return _cacheIndexContent;
        }
    }
}