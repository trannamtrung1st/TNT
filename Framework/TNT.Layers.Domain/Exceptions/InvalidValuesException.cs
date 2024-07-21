using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class InvalidValuesException : BaseException
    {
        public InvalidValuesException(params PropertyError[] errors)
            : this((IEnumerable<PropertyError>)errors)
        {
        }

        public InvalidValuesException(IEnumerable<PropertyError> errors)
            : base(ResultCode.Common.InvalidData, data: errors)
        {
        }
    }

    public class PropertyError
    {
        public PropertyError(string propertyName, string errorCode = null)
        {
            PropertyName = propertyName;
            ErrorCode = errorCode;
        }

        public string PropertyName { get; set; }
        public string ErrorCode { get; set; }
    }
}
