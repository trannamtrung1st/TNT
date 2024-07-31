using System.Linq;

namespace TNT.Boilerplates.Common.Utils
{
    public static class StringHelper
    {
        public static string JoinNonEmpty(string separator, params string[] parts)
            => string.Join(separator, parts.Where(n => !string.IsNullOrWhiteSpace(n)));
    }
}
