using System;

namespace TNT.Layers.Services.Configurations
{
    public class OpenIdInfo
    {
        public string Issuer { get; set; }
        public Uri PublicConfigurationUri { get; set; }
        public Uri[] TokenUris { get; set; }
        public Uri[] ConfigurationUris { get; set; }

        public void CopyTo(OpenIdInfo target)
        {
            target.Issuer = Issuer;
            target.PublicConfigurationUri = PublicConfigurationUri;
            target.TokenUris = TokenUris;
            target.ConfigurationUris = ConfigurationUris;
        }
    }
}