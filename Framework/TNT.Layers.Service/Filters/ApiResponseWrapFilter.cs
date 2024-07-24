using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using TNT.Boilerplates.Common.Reflection;
using TNT.Layers.Service.Attributes;
using TNT.Layers.Service.Models;

namespace TNT.Layers.Service.Filters
{
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
