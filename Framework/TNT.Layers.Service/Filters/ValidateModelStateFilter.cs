using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TNT.Layers.Service.Models;

namespace TNT.Layers.Service.Filters
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
