using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace TNT.Layers.Domain.Models
{
    public class ValueDetails
    {
        public const string Prefix = "value";

        public ValueDetails(
            string valueName, string detailCode = null,
            IReadOnlyDictionary<string, object> data = null)
            : this(valueName, new[] { detailCode }, data)
        {
        }

        public ValueDetails(
            string valueName, IEnumerable<string> detailCodes = null,
            IReadOnlyDictionary<string, object> data = null)
        {
            ValueName = valueName;
            DetailCodes = detailCodes;
            Data = data;
        }

        public string ValueName { get; }
        public IEnumerable<string> DetailCodes { get; }
        public IReadOnlyDictionary<string, object> Data { get; }

        public static IEnumerable<ValueDetails> From(ValidationResult result)
        {
            var details = new List<ValueDetails>();
            foreach (var error in result.Errors)
                details.Add(new ValueDetails(error.PropertyName, error.ErrorCode));
            return details;
        }

        public static IEnumerable<ValueDetails> From(IEnumerable<ValidationResult> results)
        {
            var details = new List<ValueDetails>();
            foreach (var result in results)
                if (!result.IsValid)
                    details.AddRange(From(result));
            return details;
        }

        public override string ToString() => $"{Prefix}:{ValueName}:{string.Join(',', DetailCodes)}";

        public static string[] GetDetails(IEnumerable<ValueDetails> details)
            => details?.Select(o => o.ToString()).ToArray();
    }
}
