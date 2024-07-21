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
            : base(ResultCode.Common.InvalidData, data: errors)
        {
        }
    }

    public class ValueError
    {
        public ValueError(string valueName, string errorCode = null)
        {
            ValueName = valueName;
            ErrorCode = errorCode;
        }

        public string ValueName { get; set; }
        public string ErrorCode { get; set; }
    }
}
