using System.Collections.Generic;
using System.Linq;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Domain.Extensions
{
    public static class ValueErrorExtensions
    {
        public static IEnumerable<ValueError> HasData(this IEnumerable<ValueError> errors)
            => errors.Where(e => e.Data != null);
    }
}
