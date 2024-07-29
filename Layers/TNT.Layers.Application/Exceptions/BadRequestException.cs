using System.Collections.Generic;
using TNT.Layers.Domain;
using TNT.Layers.Domain.Exceptions;

namespace TNT.Layers.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(IEnumerable<string> messages = null, object data = null)
            : base(ResultCodes.Common.BadRequest, messages, data)
        {
        }

        public BadRequestException(string message, object data = null)
            : base(ResultCodes.Common.BadRequest, new[] { message }, data)
        {
        }
    }
}
