using System.Security.Claims;

namespace TNT.Layers.Persistence.Services.Abstracts
{
    public interface IAuthContext<TIdentityId>
    {
        ClaimsPrincipal CurrentPrincipal { get; }
        string DisplayName { get; }
        TIdentityId IdentityId { get; }
    }
}
