﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNT.Layers.Domain.Exceptions;
using System.IO;
using Microsoft.Extensions.Options;
using TNT.Layers.Services.Configurations;
using System.Diagnostics;

namespace TNT.Layers.Services.Filters
{
    [DebuggerStepThrough]
    // [IMPORTANT] used to handle predictable exceptions, per action/controller
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IOptions<ApiExceptionFilterOptions> _options;
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(
            IOptions<ApiExceptionFilterOptions> options,
            ILogger<ApiExceptionFilter> logger)
        {
            _options = options;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            LogErrorRequestAsync(context.Exception, context.HttpContext).Wait();
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await LogErrorRequestAsync(context.Exception, context.HttpContext);
        }

        private async Task LogErrorRequestAsync(Exception ex, HttpContext httpContext)
        {
            var acceptedLevel = new[] { LogLevel.Error, LogLevel.Critical };
            if (ex is BaseException baseEx && !acceptedLevel.Contains(baseEx.LogLevel))
                return;

            if (ex is ValidationException)
                return;

            var request = httpContext.Request;
            var bodyInfo = string.Empty;

            if (request.ContentLength > 0 &&
                request.ContentLength <= _options.Value.MaxBodyLengthForLogging)
            {
                request.Body.Position = 0;
                var bodyRaw = await request.Body.ReadAsStringAsync();
                bodyInfo = string.IsNullOrEmpty(bodyRaw)
                    ? string.Empty
                    : $"{Environment.NewLine}---- Raw body ----{Environment.NewLine}{bodyRaw}{Environment.NewLine}";
            }

            Dictionary<string, StringValues> form = null;
            if (request.HasFormContentType)
                form = new Dictionary<string, StringValues>(request.Form);

            _logger.LogError("Exception on Request: {@request}{body}", new
            {
                request.ContentLength,
                request.ContentType,
                Host = request.Host.Value,
                request.IsHttps,
                request.Method,
                request.Path,
                request.Protocol,
                QueryString = request.QueryString.Value,
                request.Scheme,
                Form = form
            }, bodyInfo);
        }
    }
}
