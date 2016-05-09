using System;

namespace EAutopay
{
    /// <summary>
    /// Helps to identify the page by its URI in E-Autopay.
    /// </summary>
    internal class UriDetector
    {
        readonly Uri _uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriDetector"/> class.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        public UriDetector(Uri uri)
        {
            _uri = uri;
        }

        /// <summary>
        /// Determines whether the URI points to the "Secret Question" page E-Autopay.
        /// </summary>
        public bool IsSecretAnswerURI
        {
            get { return _uri.ToString().IndexOf("adminka/identify") > -1; }
        }

        /// <summary>
        /// Determines whether the URI points to the "Login" page E-Autopay.
        /// </summary>
        public bool IsLoginURI
        {
            get { return _uri.ToString().IndexOf("adminka/login") > -1; }
        }

        /// <summary>
        /// Determines whether the URI points to the "Main" page E-Autopay.
        /// </summary>
        public bool IsMainURI
        {
            get { return _uri.ToString().IndexOf("adminka/main") > -1; }
        }

        /// <summary>
        /// Determines whether the URI points to the "Products" page E-Autopay.
        /// </summary>
        public bool IsProdutListURI
        {
            get { return _uri.ToString().IndexOf("adminka/tovars/list_tovars") > -1; }
        }
    }
}
