using System.Collections.Generic;
using System.Linq;

namespace TNT.Layers.Domain.Utils
{
    public static class ValidationHelper
    {
        public static bool ValidateMaxLength(IEnumerable<string> values, int maxLength = CommonConstraints.MaxStringLength)
            => values.All(val => !(val?.Length > maxLength));
    }
}
