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
        public BadRequestException(params ValueDetails[] errors)
            : this((IEnumerable<ValueDetails>)errors)
        {
        }

        public BadRequestException(IEnumerable<ValidationResult> results)
            : this(ValueDetails.From(results))
        {
        }

        public BadRequestException(IEnumerable<ValueDetails> errors)
            : this(messages: ValueDetails.GetDetails(errors), errors: errors)
        {
        }

        public BadRequestException(params string[] messages)
            : this(messages: messages, errors: null)
        {
        }

        public BadRequestException(
            IEnumerable<ValueDetails> errors, IEnumerable<string> messages,
            LogLevel logLevel = LogLevel.Information
        ) : base(ResultCodes.BadRequest, details: messages, data: errors.HasData().ToArray(), logLevel)
        {
        }

        public static BadRequestException Null(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.Null));
        public static BadRequestException Empty(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.Empty));
        public static BadRequestException AboveMax(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.AboveMax));
        public static BadRequestException UnderMin(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.UnderMin));
        public static BadRequestException OutOfRange(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.OutOfRange));
        public static BadRequestException InvalidFormat(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.InvalidFormat));
        public static BadRequestException InvalidValue(string valueName)
            => new BadRequestException(new ValueDetails(valueName, DetailCodes.InvalidValue));
    }
}
