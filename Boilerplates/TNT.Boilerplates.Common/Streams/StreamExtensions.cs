using System.IO;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Common.Streams
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            return new StreamReader(stream).ReadToEndAsync();
        }
    }
}
