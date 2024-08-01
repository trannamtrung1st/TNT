using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using TNT.Layers.Domain.Extensions;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Domain.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(params ValueError[] errors)
            : this((IEnumerable<ValueError>)errors)
        {
        }

        public BadRequestException(IEnumerable<ValidationResult> results)
            : this(ValueError.From(results))
        {
        }

        public BadRequestException(IEnumerable<ValueError> errors)
            : this(messages: ValueError.GetMessages(errors), errors: errors)
        {
        }

        public BadRequestException(params string[] messages)
            : this(messages: messages, errors: null)
        {
        }

        public BadRequestException(
            IEnumerable<ValueError> errors, IEnumerable<string> messages,
            LogLevel logLevel = LogLevel.Information
        ) : base(ResultCodes.BadRequest, messages: messages, data: errors.HasData().ToArray(), logLevel)
        {
        }
    }
}
