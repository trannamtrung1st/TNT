using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace TNT.Layers.Services.Services
{
    // [IMPORTANT] process policy names by syntax: policyA[,args];policyB[,args]...
    // So we don't have to create specific policy for each endpoint, just use the syntax with generic policies defined
    // Or we can change the requirements based on configuration in database, etc.
    public abstract class BasePolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

        public BasePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies it should fall back to an alternate provider.
            _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public virtual Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();

        public virtual Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();

        public virtual async Task<AuthorizationPolicy> GetPolicyAsync(string policyId)
        {
            AuthorizationPolicy policy = await _defaultPolicyProvider.GetPolicyAsync(policyId);

            return policy;
        }
    }
}
