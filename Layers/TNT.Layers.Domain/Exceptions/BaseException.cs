using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace TNT.Layers.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(
            string resultCode,
            IEnumerable<string> details = null,
            object data = null,
            LogLevel logLevel = LogLevel.Error)
        {
            Code = resultCode;
            DataObject = data;
            Details = details;
            LogLevel = logLevel;
        }

        public IEnumerable<string> Details { get; }
        public override string Message
        {
            get
            {
                if (Details != null)
                    return string.Join(Environment.NewLine, Details);
                return base.Message;
            }
        }
        public object DataObject { get; }
        public string Code { get; }
        public LogLevel LogLevel { get; }
    }
}
