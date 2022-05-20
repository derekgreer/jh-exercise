using System;
using System.Net;

namespace JHExercise.Specs.Infrastructure.Clients
{
    public class WebServiceClientResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
    }

    public class WebServiceClientResponse<TResponse> : WebServiceClientResponse
    {
        public TResponse Response { get; set; }
        public Exception Exception { get; set; }
    }
}