using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using TNT.Boilerplates.Validation.Models;

namespace TNT.Boilerplates.Validation
{
    public class SimpleValidator<TValue> : IValidator<TValue>
    {
        private readonly IValidator<WrappedValue<TValue>> _validator;

        public SimpleValidator(Action<InlineValidator<WrappedValue<TValue>>> configure)
        {
            var inlineValidator = new InlineValidator<WrappedValue<TValue>>();
            configure(inlineValidator);
            _validator = inlineValidator;
        }

        public IValidator<WrappedValue<TValue>> WrappedValidator => _validator;

        public bool CanValidateInstancesOfType(Type type) => _validator.CanValidateInstancesOfType(type);

        public IValidatorDescriptor CreateDescriptor() => _validator.CreateDescriptor();

        public ValidationResult Validate(TValue instance) => _validator.Validate(new WrappedValue<TValue>(instance));

        public ValidationResult Validate(IValidationContext context) => _validator.Validate(context);

        public Task<ValidationResult> ValidateAsync(TValue instance, CancellationToken cancellation = default)
            => _validator.ValidateAsync(new WrappedValue<TValue>(instance), cancellation);

        public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
            => _validator.ValidateAsync(context, cancellation);
    }
}