using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TNT.Boilerplates.Crypto.Abstracts;

namespace TNT.Boilerplates.Crypto
{
    public class TokenService : ITokenService
    {
        public TokenService()
        {
        }

        public string GenerateToken(IEnumerable<Claim> claims,
            string secret, string issuer, string aud, int ttl)
        {
            var expires = DateTime.UtcNow.AddSeconds(ttl);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, aud, claims,
                notBefore: null, expires: expires, signingCredentials: credentials);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }

        public bool ValidateToken(string token, string secret, string issuer, string aud,
            out ClaimsPrincipal claimsPrincipal,
            out Exception validateException,
            TimeSpan? clockSkew = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = aud,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            };

            if (clockSkew != null) { validationParameters.ClockSkew = clockSkew.Value; }

            claimsPrincipal = null;
            validateException = null;
            SecurityToken validatedToken;
            bool valid;

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                valid = validatedToken != null;
            }
            catch (Exception ex)
            {
                validateException = ex;
                valid = false;
            }

            return valid;
        }

    }
}
