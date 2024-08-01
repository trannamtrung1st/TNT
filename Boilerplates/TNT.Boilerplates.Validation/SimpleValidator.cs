using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using TNT.Boilerplates.Common.Utils;

namespace TNT.Boilerplates.Validation
{
    public class SimpleValidator<TValue> : IValidator<TValue>
    {
        private readonly IValidator<ValueWrap<TValue>> _validator;

        public SimpleValidator(Action<InlineValidator<ValueWrap<TValue>>> configure)
        {
            var inlineValidator = new InlineValidator<ValueWrap<TValue>>();
            configure(inlineValidator);
            _validator = inlineValidator;
        }

        public SimpleValidator(params IValidator<ValueWrap<TValue>>[] validators)
        {
            var inlineValidator = new InlineValidator<ValueWrap<TValue>>();
            _validator = inlineValidator;

            foreach (var validator in validators)
                inlineValidator.Include(validator);
        }

        public IValidator<ValueWrap<TValue>> InnerValidator => _validator;

        public bool CanValidateInstancesOfType(Type type) => _validator.CanValidateInstancesOfType(type);

        public IValidatorDescriptor CreateDescriptor() => _validator.CreateDescriptor();

        public ValidationResult Validate(TValue instance) => _validator.Validate(new ValueWrap<TValue>(instance));

        public ValidationResult Validate(IValidationContext context) => _validator.Validate(context);

        public Task<ValidationResult> ValidateAsync(TValue instance, CancellationToken cancellation = default)
            => _validator.ValidateAsync(new ValueWrap<TValue>(instance), cancellation);

        public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
            => _validator.ValidateAsync(context, cancellation);
    }
}