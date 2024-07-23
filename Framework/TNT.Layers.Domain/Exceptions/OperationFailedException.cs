using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class OperationFailedException : BaseException
    {
        public OperationFailedException(object data = null, IEnumerable<string> messages = null)
            : base(ResultCodes.Common.OperationFailed, messages, data, LogLevel.Error)
        {
        }
    }
}
