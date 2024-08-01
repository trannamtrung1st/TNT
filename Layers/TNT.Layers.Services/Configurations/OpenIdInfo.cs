namespace TNT.Layers.Services.Configurations
{
    public class OpenIdInfo
    {
        public static class DefaultEndpoints
        {
            public const string Configuration = "/.well-known/openid-configuration";
            public const string Authorization = "/connect/authorize";
            public const string Token = "/connect/token";
            public const string Cryptography = "/.well-known/jwks";
        }

        #region Server

        public string Issuer { get; set; }
        public string ServicePrefix { get; set; }
        public string PublicConfigurationEndpoint { get; set; }
        public string[] AuthorizationEndpoints { get; set; }
        public string[] TokenEndpoints { get; set; }
        public string[] ConfigurationEndpoints { get; set; }
        public string[] CryptographyEndpoints { get; set; }
        public string IdpPublicUrl { get; set; }

        #endregion Server

        #region Client

        public string[] ValidAudiences { get; set; }

        #endregion Client

        public void CopyTo(OpenIdInfo target)
        {
            target.Issuer = Issuer;
            target.ServicePrefix = ServicePrefix;
            target.PublicConfigurationEndpoint = PublicConfigurationEndpoint;
            target.AuthorizationEndpoints = AuthorizationEndpoints;
            target.TokenEndpoints = TokenEndpoints;
            target.ConfigurationEndpoints = ConfigurationEndpoints;
            target.CryptographyEndpoints = CryptographyEndpoints;
            target.IdpPublicUrl = IdpPublicUrl;
        }

        public void UseDefaults()
        {
            PublicConfigurationEndpoint = DefaultEndpoints.Configuration;
            ConfigurationEndpoints = new[] { PublicConfigurationEndpoint };
            AuthorizationEndpoints = new[] { DefaultEndpoints.Authorization };
            TokenEndpoints = new[] { DefaultEndpoints.Token };
            CryptographyEndpoints = new[] { DefaultEndpoints.Cryptography };
        }

        public void UseDefaultsWithPrefix()
        {
            var prefix = ServicePrefix != null ? $"/{ServicePrefix}" : null;
            PublicConfigurationEndpoint = $"{prefix}{DefaultEndpoints.Configuration}";
            ConfigurationEndpoints = new[]
            {
                PublicConfigurationEndpoint,
                DefaultEndpoints.Configuration
            };
            AuthorizationEndpoints = new[]
            {
                $"{prefix}{DefaultEndpoints.Authorization}",
                DefaultEndpoints.Authorization
            };
            TokenEndpoints = new[]
            {
                $"{prefix}{DefaultEndpoints.Token}",
                DefaultEndpoints.Token
            };
            CryptographyEndpoints = new[]
            {
                $"{prefix}{DefaultEndpoints.Cryptography}",
                DefaultEndpoints.Cryptography
            };
        }
    }
}