using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class UnknownException : BaseException
    {
        public UnknownException(object data = null, IEnumerable<string> messages = null)
            : base(ResultCodes.Common.UnknownError, messages, data, LogLevel.Error)
        {
        }
    }
}
