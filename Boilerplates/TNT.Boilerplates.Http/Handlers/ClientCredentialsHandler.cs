using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Http.Handlers
{
    public class ClientCredentialsHandler : DelegatingHandler
    {
        public ClientCredentialsHandler()
        {
        }

        public ClientCredentialsHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // [TODO] with OpenIdDict
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
