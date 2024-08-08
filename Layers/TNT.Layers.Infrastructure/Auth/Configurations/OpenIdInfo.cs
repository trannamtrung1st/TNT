namespace TNT.Layers.Infrastructure.Auth.Configurations
{
    public class OpenIdInfo
    {
        public static class DefaultEndpoints
        {
            public const string Configuration = "/.well-known/openid-configuration";
            public const string Authorization = "/connect/authorize";
            public const string Token = "/connect/token";
            public const string Logout = "/connect/logout";
            public const string UserInfo = "/connect/userinfo";
            public const string Cryptography = "/.well-known/jwks";
        }

        #region Server

        public string Issuer { get; set; }
        public string PublicConfigurationEndpoint { get; set; }
        public string[] AuthorizationEndpoints { get; set; }
        public string[] TokenEndpoints { get; set; }
        public string[] ConfigurationEndpoints { get; set; }
        public string[] CryptographyEndpoints { get; set; }
        public string[] LogoutEndpoints { get; set; }
        public string[] UserInfoEndpoints { get; set; }
        public string IdpPublicUrl { get; set; }
        public bool UseReferenceAccessTokens { get; set; }
        public bool UseReferenceRefreshTokens { get; set; }

        #endregion Server

        #region Client

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] ValidAudiences { get; set; }
        public bool UseTokenEntryValidation { get; set; }
        public bool UseAuthorizationEntryValidation { get; set; }
        public bool UseIntrospection { get; set; }
        public string[] Scopes { get; set; }

        #endregion Client

        public void CopyTo(OpenIdInfo target)
        {
            target.Issuer = Issuer;
            target.PublicConfigurationEndpoint = PublicConfigurationEndpoint;
            target.AuthorizationEndpoints = AuthorizationEndpoints;
            target.TokenEndpoints = TokenEndpoints;
            target.ConfigurationEndpoints = ConfigurationEndpoints;
            target.CryptographyEndpoints = CryptographyEndpoints;
            target.LogoutEndpoints = LogoutEndpoints;
            target.UserInfoEndpoints = UserInfoEndpoints;
            target.IdpPublicUrl = IdpPublicUrl;
            target.UseReferenceAccessTokens = UseReferenceAccessTokens;
            target.UseReferenceRefreshTokens = UseReferenceRefreshTokens;

            target.ClientId = ClientId;
            target.ClientSecret = ClientSecret;
            target.ValidAudiences = ValidAudiences;
            target.UseTokenEntryValidation = UseTokenEntryValidation;
            target.UseAuthorizationEntryValidation = UseAuthorizationEntryValidation;
            target.UseIntrospection = UseIntrospection;
            target.Scopes = Scopes;
        }

        public void UseDefaults()
        {
            PublicConfigurationEndpoint = DefaultEndpoints.Configuration;
            ConfigurationEndpoints = new[] { PublicConfigurationEndpoint };
            AuthorizationEndpoints = new[] { DefaultEndpoints.Authorization };
            TokenEndpoints = new[] { DefaultEndpoints.Token };
            CryptographyEndpoints = new[] { DefaultEndpoints.Cryptography };
            LogoutEndpoints = new[] { DefaultEndpoints.Logout };
            UserInfoEndpoints = new[] { DefaultEndpoints.UserInfo };
        }
    }
}