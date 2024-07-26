using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Services.Services
{
    public abstract class HttpAuthContext<TIdentityId> : IAuthContext<TIdentityId>
    {
        protected readonly IHttpContextAccessor httpContextAccessor;

        public HttpAuthContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public virtual ClaimsPrincipal CurrentPrincipal => httpContextAccessor.HttpContext?.User;
        public virtual string DisplayName => CurrentPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        public abstract TIdentityId IdentityId { get; }
    }
}
