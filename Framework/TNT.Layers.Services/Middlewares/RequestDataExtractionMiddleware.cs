using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TNT.Layers.Persistence.Services.Abstracts;
using TNT.Layers.Services.Services.Abstracts;

namespace TNT.Layers.Services.Middlewares
{
    public class RequestDataExtractionMiddleware<TIdentityId> : IMiddleware
    {
        private readonly IAuthContext<TIdentityId> _authContext;
        private readonly IRequestContext _requestContext;

        public RequestDataExtractionMiddleware(
            IAuthContext<TIdentityId> authContext,
            IRequestContext requestContext)
        {
            _authContext = authContext;
            _requestContext = requestContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!string.IsNullOrEmpty(_authContext.DisplayName))
            {
                _requestContext.Set(RequestContextKeys.DisplayName, _authContext.DisplayName);
                _requestContext.Set(RequestContextKeys.IdentityId, _authContext.IdentityId);
            }

            await next(context);
        }
    }
}
