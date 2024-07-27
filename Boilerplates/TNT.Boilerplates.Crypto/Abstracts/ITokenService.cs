using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TNT.Boilerplates.Crypto.Abstracts
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims,
            string secret, string issuer, string aud, int ttl);

        bool ValidateToken(string token, string secret, string issuer, string aud,
            out ClaimsPrincipal claimsPrincipal,
            out Exception validateException,
            TimeSpan? clockSkew = null);
    }
}
