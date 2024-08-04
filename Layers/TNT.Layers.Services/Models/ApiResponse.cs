using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TNT.Layers.Domain;
using TNT.Layers.Domain.Exceptions;
using TNT.Layers.Domain.Extensions;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Services.Models
{
    public class ApiResponse
    {
        protected ApiResponse(string code, IEnumerable<string> details = null, object data = null)
        {
            Code = code;
            Details = details;
            Data = data;
        }

        public string Code { get; }
        public IEnumerable<string> Details { get; }
        public object Data { get; }

        [JsonExtensionData]
        public IDictionary<string, JsonElement> Extensions { get; set; } = new Dictionary<string, JsonElement>();

        public static ApiResponse From(ResultModel result)
            => new ApiResponse(result.Code, result.Details, result.Data);

        public static ApiResponse Object(object data, IEnumerable<string> details = null)
            => new ApiResponse(ResultCodes.ObjectResult, details, data);

        public static ApiResponse BadRequest(object data = null, IEnumerable<string> details = null)
            => new ApiResponse(ResultCodes.BadRequest, details, data);

        public static ApiResponse BadRequest(ValidationException validationException)
        {
            var errors = validationException.Errors
                .Select(o => new ValueDetails(valueName: o.PropertyName, detail: o.ErrorCode))
                .ToArray();
            var details = ValueDetails.GetDetails(errors);
            var apiResponse = BadRequest(data: errors.HasData().ToArray(), details: details);
            return apiResponse;
        }

        public static ApiResponse BadRequest(ArgumentException argumentException)
        {
            var details = ValueDetails.GetDetails(new ValueDetails(
                valueName: argumentException.ParamName,
                detail: argumentException.Message));
            var apiResponse = BadRequest(details: details);
            return apiResponse;
        }

        public static ApiResponse BadRequest(ModelStateDictionary modelState)
        {
            var validationErrors = modelState.Values
                .SelectMany(o => o.Errors)
                .Select(o => new ValueDetails(valueName: nameof(modelState), detail: o.ErrorMessage))
                .ToArray();
            var apiResponse = BadRequest(validationErrors);
            return apiResponse;
        }

        public static ApiResponse Exception(BaseException exception)
            => new ApiResponse(code: exception.Code, details: exception.Details, data: exception.DataObject);

        public static ApiResponse NotFound(object data = null, IEnumerable<string> details = null)
            => new ApiResponse(ResultCodes.NotFound, details, data);

        public static ApiResponse UnknownError(object data = null, IEnumerable<string> details = null)
            => new ApiResponse(ResultCodes.UnknownError, details, data);
    }
}
