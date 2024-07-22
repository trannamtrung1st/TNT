using System;
using FluentValidation;
using TNT.Boilerplates.Validation.Abstracts;
using TNT.Boilerplates.Validation.Models;

namespace TNT.Boilerplates.Validation
{
    public class SimpleValidatorFactory : ISimpleValidatorFactory
    {
        public IValidator<TValue> CreateValidator<TValue>(Action<InlineValidator<WrappedValue<TValue>>> configure)
            => new SimpleValidator<TValue>(configure);
    }
}