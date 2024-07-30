using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TNT.Layers.Services.Validators
{
    public class NullObjectModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
        {
        }
    }
}