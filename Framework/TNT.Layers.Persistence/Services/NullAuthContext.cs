using System.Security.Claims;
using TNT.Layers.Persistence.Services.Abstracts;

namespace TNT.Layers.Persistence.Services
{
    public class NullAuthContext<TIdentityId> : IAuthContext<TIdentityId>
    {
        public ClaimsPrincipal CurrentPrincipal { get; }
        public TIdentityId IdentityId { get; }
    }
}
