using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(
            object data = null, IEnumerable<string> messages = null,
            LogLevel logLevel = LogLevel.Information
        ) : base(ResultCodes.NotFound, messages, data, logLevel)
        {
        }
    }
}
