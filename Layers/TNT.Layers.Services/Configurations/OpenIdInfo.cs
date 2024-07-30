using System;

namespace TNT.Layers.Services.Configurations
{
    public class OpenIdInfo
    {
        public string Issuer { get; set; }
        public Uri PublicConfigurationUri { get; set; }
        public Uri[] TokenUris { get; set; }
        private Uri[] _configurationUris;
        public Uri[] ConfigurationUris
        {
            get
            {
                if (_configurationUris == null && PublicConfigurationUri != null)
                    _configurationUris = new[] { PublicConfigurationUri };
                return _configurationUris;
            }
            set => _configurationUris = value;
        }

        public void CopyTo(OpenIdInfo target)
        {
            target.Issuer = Issuer;
            target.PublicConfigurationUri = PublicConfigurationUri;
            target.TokenUris = TokenUris;
            target.ConfigurationUris = ConfigurationUris;
        }
    }
}