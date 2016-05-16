using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace EAutopay
{
    /// <summary>
    /// Provides underlying operations for the crawler.
    /// </summary>
    internal class CrawlerService
    {
        /// <summary>
        /// Writes the specified data to the specified <see cref="HttpWebRequest"/> object.
        /// </summary>
        /// <param name="request"><see cref="HttpWebRequest"/> to write data to.</param>
        /// <param name="paramz">The data to be posted.</param>
        /// <param name="token">The E-Autopay token.</param>
        public static void WritePostData(HttpWebRequest request, NameValueCollection paramz, string token)
        {
            using (var stream = request.GetRequestStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(BuildPostData(paramz, token));
                
            }
        }

        /// <summary>
        /// Combines base uri with querystring params.
        /// </summary>
        /// <param name="uri">Base uri.</param>
        /// <param name="paramz">Querystring params.</param>
        /// <returns>URI as string.</returns>
        public static string CombineUriWithParams(string uri, NameValueCollection paramz)
        {
            if (paramz != null && paramz.Count > 0)
            {
                uri += "?";
                foreach (string p in paramz)
                {
                    uri += string.Format("{0}={1}&", p, paramz[p]);
                }
                uri = uri.Trim('&');
            }
            return uri;
        }

        /// <summary>
        /// Generates post data based on the specified paramz and token.
        /// </summary>
        /// <param name="paramz">The data to be posted.</param>
        /// <param name="token">The E-Autopay token.</param>
        /// <returns>Data to be posted as string.</returns>
        internal static string BuildPostData(NameValueCollection paramz, string token)
        {
            string ret = "_token="+ token;

            if (paramz !=null && paramz.Count > 0)
            {
                foreach (string p in paramz)
                {
                    ret += string.Format("&{0}={1}", p, paramz[p]);
                }
            }
            return ret;
        }
    }
}
