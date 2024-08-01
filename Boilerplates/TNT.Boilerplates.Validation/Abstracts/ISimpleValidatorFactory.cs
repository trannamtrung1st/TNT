using System;
using FluentValidation;
using TNT.Boilerplates.Common.Utils;

namespace TNT.Boilerplates.Validation.Abstracts
{
    public interface ISimpleValidatorFactory
    {
        IValidator<TValue> CreateValidator<TValue>(Action<InlineValidator<ValueWrap<TValue>>> configure);
    }
}