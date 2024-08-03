using System.Collections.Generic;
using System.Linq;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Domain.Extensions
{
    public static class ValueDetailssExtensions
    {
        public static IEnumerable<ValueDetails> HasData(this IEnumerable<ValueDetails> errors)
            => errors.Where(e => e.Data != null);
    }
}
