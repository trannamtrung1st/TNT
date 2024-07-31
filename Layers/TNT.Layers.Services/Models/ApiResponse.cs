using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TNT.Layers.Domain;
using TNT.Layers.Domain.Exceptions;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Services.Models
{
    public class ApiResponse
    {
        protected ApiResponse(string code, IEnumerable<string> messages = null, object data = null)
        {
            Code = code;
            Messages = messages;
            Data = data;
        }

        public string Code { get; }
        public IEnumerable<string> Messages { get; }
        public object Data { get; }

        [JsonExtensionData]
        public IDictionary<string, JsonElement> Extensions { get; set; } = new Dictionary<string, JsonElement>();

        public static ApiResponse From(ResultModel result)
            => new ApiResponse(result.Code, result.Messages, result.Data);

        public static ApiResponse Object(object data, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.ObjectResult, messages, data);

        public static ApiResponse BadRequest(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.BadRequest, messages, data);

        public static ApiResponse BadRequest(ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                .Select(o => new ValueError(valueName: o.PropertyName, errorCode: o.ErrorCode))
                .ToArray();
            var apiResponse = BadRequest(validationErrors);
            return apiResponse;
        }

        public static ApiResponse BadRequest(ModelStateDictionary modelState)
        {
            var validationErrors = modelState.Values
                .SelectMany(o => o.Errors)
                .Select(o => new ValueError(valueName: nameof(modelState), errorCode: o.ErrorMessage))
                .ToArray();
            var apiResponse = BadRequest(validationErrors);
            return apiResponse;
        }

        public static ApiResponse Exception(BaseException exception)
            => new ApiResponse(exception.Code, exception.Messages, exception.DataObject);

        public static ApiResponse NotFound(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.NotFound, messages, data);

        public static ApiResponse UnknownError(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.UnknownError, messages, data);
    }
}
