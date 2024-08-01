using System;
using FluentValidation;
using TNT.Boilerplates.Common.Utils;
using TNT.Boilerplates.Validation.Abstracts;

namespace TNT.Boilerplates.Validation
{
    public class SimpleValidatorFactory : ISimpleValidatorFactory
    {
        public IValidator<TValue> CreateValidator<TValue>(Action<InlineValidator<ValueWrap<TValue>>> configure)
            => new SimpleValidator<TValue>(configure);

        public IValidator<TValue> CreateValidator<TValue>(params IValidator<ValueWrap<TValue>>[] validators)
            => new SimpleValidator<TValue>(validators);
    }
}