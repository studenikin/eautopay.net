using System;
using System.Net;

namespace EAutopay
{
    // Encapsulates the HTTP response from E-Autopay.
    public class EAutopayResponse
    {
        /// <summary>
        /// Gets the source of the HTTP response.
        /// </summary>
        public string Data { get; internal set; }

        /// <summary>
        /// Get the URI of the HTTP response.
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Gets the status code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }
    }
} 
