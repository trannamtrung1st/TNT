using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace TNT.Layers.Services.AuthHandlers.Client
{
    public class ClientAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<ApplicationClient> Clients { get; set; }
    }

    public class ApplicationClient
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
