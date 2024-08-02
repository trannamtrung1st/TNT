using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace TNT.Layers.Services.Controllers
{
    [ApiController]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult Error(object obj = default)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, obj);
        }

        protected IActionResult AccessDenied(object obj = default)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, obj);
        }

        protected string GetAuthority()
        {
            return new Uri(Request.GetEncodedUrl()).Authority;
        }
    }
}
