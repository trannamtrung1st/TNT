using System;
using FluentValidation;
using TNT.Boilerplates.Validation.Models;

namespace TNT.Boilerplates.Validation.Abstracts
{
    public interface ISimpleValidatorFactory
    {
        IValidator<TValue> CreateValidator<TValue>(Action<InlineValidator<WrappedValue<TValue>>> configure);
    }
}