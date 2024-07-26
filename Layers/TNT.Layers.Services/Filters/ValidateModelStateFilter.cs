using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TNT.Layers.Services.Models;

namespace TNT.Layers.Services.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var apiResponse = ApiResponse.BadRequest(context.ModelState);
            context.Result = new BadRequestObjectResult(apiResponse);
        }
    }
}
