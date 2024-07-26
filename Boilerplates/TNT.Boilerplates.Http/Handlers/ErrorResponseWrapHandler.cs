using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TNT.Boilerplates.Http.Exceptions;

namespace TNT.Boilerplates.Http.Handlers
{
    public class ErrorResponseWrapHandler : DelegatingHandler
    {
        public ErrorResponseWrapHandler()
        {
        }

        public ErrorResponseWrapHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await base.SendAsync(request, cancellationToken);

                response = response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                throw new HttpException(response, ex);
            }
        }
    }
}
