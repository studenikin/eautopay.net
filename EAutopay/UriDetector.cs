using System;

namespace EAutopay
{
    internal class UriDetector
    {
        Uri _uri;

        public UriDetector(Uri uri)
        {
            _uri = uri;
        }

        public bool IsSecretAnswerURI()
        {
            return _uri.ToString().IndexOf("identify") > -1;
        }

        public bool IsLoginURI()
        {
            return _uri.ToString().IndexOf("login") > -1;
        }

        public bool IsMainURI()
        {
            return _uri.ToString().IndexOf("main") > -1;
        }

        public bool IsProdutListURI()
        {
            return _uri.ToString().IndexOf("list_tovars") > -1;
        }
    }
}
