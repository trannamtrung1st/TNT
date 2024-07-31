using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TNT.Boilerplates.Common.Utils;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Services.Services
{
    public abstract class BaseHttpAuthContext<TIdentityId> : IAuthContext<TIdentityId>
    {
        protected readonly IHttpContextAccessor httpContextAccessor;

        public BaseHttpAuthContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public virtual ClaimsPrincipal CurrentPrincipal => httpContextAccessor.HttpContext?.User;
        public virtual string DisplayName
        {
            get
            {
                var givenName = CurrentPrincipal?.FindFirstValue(Claims.GivenName);
                var middleName = CurrentPrincipal?.FindFirstValue(Claims.MiddleName);
                var familyName = CurrentPrincipal?.FindFirstValue(Claims.FamilyName);
                return StringHelper.JoinNonEmpty(givenName, middleName, familyName);
            }
        }
        public abstract TIdentityId IdentityId { get; }
    }
}
