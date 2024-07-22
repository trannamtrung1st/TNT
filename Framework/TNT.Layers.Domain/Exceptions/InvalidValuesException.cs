using System.Collections.Generic;
using FluentValidation.Results;

namespace TNT.Layers.Domain.Exceptions
{
    public class InvalidValuesException : BaseException
    {
        public InvalidValuesException(params ValueError[] errors)
            : this((IEnumerable<ValueError>)errors)
        {
        }

        public InvalidValuesException(IEnumerable<ValidationResult> results)
            : this(ValueError.From(results))
        {
        }

        public InvalidValuesException(IEnumerable<ValueError> errors)
            : base(ResultCodes.Common.InvalidData, data: errors)
        {
        }
    }

    public class ValueError
    {
        public ValueError(
            string valueName, string errorCode = null,
            IReadOnlyDictionary<string, object> data = null)
        {
            ValueName = valueName;
            ErrorCode = errorCode;
            Data = data;
        }

        public string ValueName { get; }
        public string ErrorCode { get; }
        public IReadOnlyDictionary<string, object> Data { get; }

        public static IEnumerable<ValueError> From(ValidationResult result)
        {
            var errors = new List<ValueError>();
            foreach (var error in result.Errors)
                errors.Add(new ValueError(error.PropertyName, error.ErrorCode));
            return errors;
        }

        public static IEnumerable<ValueError> From(IEnumerable<ValidationResult> results)
        {
            var errors = new List<ValueError>();
            foreach (var result in results)
                if (!result.IsValid)
                    errors.AddRange(From(result));
            return errors;
        }
    }
}
