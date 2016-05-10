using System;
using System.Net;

namespace EAutopay
{
    public class EAutopayResponse
    {
        public string Data { get; internal set; }

        public Uri Uri { get; internal set; }

        public HttpStatusCode StatusCode { get; internal set; }
    }
} 
