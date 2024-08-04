using System;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TNT.Layers.Domain.Exceptions;
using TNT.Layers.Services.Models;

namespace TNT.Layers.Services.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseErrorController : BaseApiController
    {
        protected readonly IWebHostEnvironment env;

        public BaseErrorController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        protected virtual IActionResult HandleCommonException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            if (exception == null)
                return BadRequest(ApiResponse.Exception(
                    BadRequestException.Null(nameof(exception))));

            ApiResponse response = null;
            if (exception is NotFoundException notFoundEx)
            {
                response = ApiResponse.Exception(notFoundEx);
                return NotFound(response);
            }
            else if (exception is AccessDeniedException accessDeniedEx)
            {
                response = ApiResponse.Exception(accessDeniedEx);
                return AccessDenied(response);
            }
            else if (exception is ValidationException validationEx)
            {
                response = ApiResponse.BadRequest(validationEx);
                return BadRequest(response);
            }
            else if (exception is ArgumentException argumentEx)
            {
                response = ApiResponse.BadRequest(argumentEx);
                return BadRequest(response);
            }
            else if (exception is BaseException baseEx)
            {
                response = ApiResponse.Exception(baseEx);
                return BadRequest(response);
            }

            if (response == null)
            {
                if (env.IsDevelopment())
                    response = ApiResponse.UnknownError(details: new[] { exception.Message });
                else response = ApiResponse.UnknownError();
            }

            return Error(response);
        }
    }
}
