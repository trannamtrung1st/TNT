using System;
using System.Net.Http;

namespace TNT.Boilerplates.Http.Exceptions
{
    public class HttpException : Exception
    {
        public HttpResponseMessage Response { get; }
        public Exception Exception { get; }

        public HttpException(HttpResponseMessage response, Exception ex)
        {
            Response = response;
            Exception = ex;
        }
    }
}
