using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class AccessDeniedException : BaseException
    {
        public AccessDeniedException(
            string resultCode = ResultCodes.Common.AccessDenied,
            IEnumerable<string> messages = null, object data = null,
            LogLevel logLevel = LogLevel.Error) : base(resultCode, messages, data, logLevel)
        {
        }
    }
}
