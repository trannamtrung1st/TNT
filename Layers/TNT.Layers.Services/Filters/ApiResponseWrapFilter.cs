using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TNT.Boilerplates.Common.Reflection;
using TNT.Layers.Services.Attributes;
using TNT.Layers.Services.Models;

namespace TNT.Layers.Services.Filters
{
    [DebuggerStepThrough]
    public class ApiResponseWrapFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controllerAction = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerAction == null)
                return;

            var objectResult = context.Result as ObjectResult;
            if (objectResult == null)
                return;

            if (!typeof(Task<IActionResult>).IsAssignableFrom(controllerAction.MethodInfo.ReturnType)
                && !typeof(IActionResult).IsAssignableFrom(controllerAction.MethodInfo.ReturnType))
                return;

            var hasNoWrap = ReflectionHelper.GetAttributesOfMemberOrType<NoWrapAttribute>(controllerAction.MethodInfo).Any();
            if (hasNoWrap)
                return;

            if (objectResult.Value is not ApiResponse)
                objectResult.Value = ApiResponse.Object(objectResult.Value);
        }
    }
}
