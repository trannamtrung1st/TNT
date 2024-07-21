using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public abstract class TypedDataException<T> : BaseException
    {
        public T TypedDataObject => (T)DataObject;

        protected TypedDataException(
            string resultCode,
            IEnumerable<string> messages = null,
            T data = default,
            LogLevel logLevel = LogLevel.Error) : base(resultCode, messages, data, logLevel)
        {
        }
    }
}
