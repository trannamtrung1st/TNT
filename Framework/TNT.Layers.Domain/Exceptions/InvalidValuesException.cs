using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class InvalidValuesException : BaseException
    {
        public InvalidValuesException(params ValueError[] errors)
            : this((IEnumerable<ValueError>)errors)
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
    }
}
