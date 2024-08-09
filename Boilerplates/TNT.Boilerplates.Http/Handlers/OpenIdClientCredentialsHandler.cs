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
using static OpenIddict.Abstractions.OpenIddictExceptions;
using static OpenIddict.Client.OpenIddictClientModels;

namespace TNT.Boilerplates.Http.Handlers
{
    public class OpenIdClientCredentialsHandler : DelegatingHandler
    {
        private readonly IOptions<OpenIdClientCredentialsHandlerOptions> _options;
        private readonly OpenIddictClientService _client;
        private string _accessToken;
        private string _refreshToken;
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
            {
                _accessToken ??= await GetTokenByClientCredentials(cancellationToken);
                SetAuthorizationHeader(request, accessToken: _accessToken);
            }
            else if (DateTime.UtcNow >= _accessTokenExpiry)
            {
                _accessToken = await RefreshToken(refreshToken: _refreshToken, cancellationToken);
                SetAuthorizationHeader(request, accessToken: _accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _accessToken = await RefreshToken(refreshToken: _refreshToken, cancellationToken);
                SetAuthorizationHeader(request, accessToken: _accessToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return response;
        }

        protected virtual void SetAuthorizationHeader(HttpRequestMessage request, string accessToken)
            => request.Headers.Authorization = new AuthenticationHeaderValue(OpenIddictConstants.Schemes.Bearer, accessToken);

        protected virtual async Task<string> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return await GetTokenByClientCredentials(cancellationToken);

            try
            {
                var result = await _client.AuthenticateWithRefreshTokenAsync(new RefreshTokenAuthenticationRequest()
                {
                    RefreshToken = refreshToken,
                    Scopes = _options.Value.Scopes.ToList(),
                    DisableUserinfo = true
                });

                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    _refreshToken = result.RefreshToken;
                    _accessTokenExpiry = result.AccessTokenExpirationDate;
                    return result.AccessToken;
                }
            }
            catch (ProtocolException) { }

            return await GetTokenByClientCredentials(cancellationToken);
        }

        protected virtual async Task<string> GetTokenByClientCredentials(CancellationToken cancellationToken)
        {
            var result = await _client.AuthenticateWithClientCredentialsAsync(new()
            {
                CancellationToken = cancellationToken,
                Scopes = _options.Value.Scopes.ToList()
            });
            _refreshToken = result.RefreshToken;
            _accessTokenExpiry = result.AccessTokenExpirationDate;
            return result.AccessToken;
        }
    }
}
