using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            return new StreamReader(stream).ReadToEndAsync();
        }
    }
}
