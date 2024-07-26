using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(
            string resultCode,
            IEnumerable<string> messages = null,
            object data = null,
            LogLevel logLevel = LogLevel.Error)
        {
            Code = resultCode;
            DataObject = data;
            Messages = messages;
            LogLevel = logLevel;
        }

        public IEnumerable<string> Messages { get; }
        public override string Message => string.Join(Environment.NewLine, Messages);
        public object DataObject { get; }
        public string Code { get; }
        public LogLevel LogLevel { get; }
    }
}
