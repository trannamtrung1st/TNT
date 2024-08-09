using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using TNT.Boilerplates.Http.Configurations;

namespace TNT.Boilerplates.Http.Handlers
{
    public class OpenIdClientCredentialsHandler : DelegatingHandler
    {
        private readonly IOptions<OpenIdClientCredentialsHandlerOptions> _options;
        private readonly OpenIddictClientService _client;
        private string _accessToken;
        private DateTimeOffset? _accessTokenExpiry;

        public OpenIdClientCredentialsHandler(IOptions<OpenIdClientCredentialsHandlerOptions> options, OpenIddictClientService client)
        {
            _options = options;
            _client = client;
        }

        public OpenIdClientCredentialsHandler(IOptions<OpenIdClientCredentialsHandlerOptions> options, HttpMessageHandler innerHandler, OpenIddictClientService client) : base(innerHandler)
        {
            _options = options;
            _client = client;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Headers.Authorization?.Parameter))
                _accessToken ??= await GetTokenByClientCredentials(cancellationToken);
            else if (DateTime.UtcNow >= _accessTokenExpiry)
                _accessToken = await GetTokenByClientCredentials(cancellationToken);

            SetAuthorizationHeader(request, accessToken: _accessToken);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _accessToken = await GetTokenByClientCredentials(cancellationToken);
                SetAuthorizationHeader(request, accessToken: _accessToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return response;
        }

        protected virtual void SetAuthorizationHeader(HttpRequestMessage request, string accessToken)
            => request.Headers.Authorization = new AuthenticationHeaderValue(OpenIddictConstants.Schemes.Bearer, accessToken);

        protected virtual async Task<string> GetTokenByClientCredentials(CancellationToken cancellationToken)
        {
            var result = await _client.AuthenticateWithClientCredentialsAsync(new()
            {
                CancellationToken = cancellationToken,
                Scopes = _options.Value.Scopes?.ToList()
            });
            _accessTokenExpiry = result.AccessTokenExpirationDate;
            return result.AccessToken;
        }
    }
}
