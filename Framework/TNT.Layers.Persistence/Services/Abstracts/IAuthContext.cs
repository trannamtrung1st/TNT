using System.Security.Claims;

namespace TNT.Layers.Persistence.Services.Abstracts
{
    public interface IAuthContext<TIdentityId>
    {
        ClaimsPrincipal CurrentPrincipal { get; }
        TIdentityId IdentityId { get; }
    }
}
