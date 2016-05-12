using System.Collections.Specialized;

namespace EAutopay
{
    /// <summary>
    /// Provides a way to post/get http request from/to E-Autopay.
    /// </summary>
    public interface ICrawler
    {
        /// <summary>
        /// Gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        EAutopayResponse Get(string uri);

        /// <summary>
        /// Gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <param name="paramz">The data to send to the page.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        EAutopayResponse Get(string uri, NameValueCollection paramz);

        /// <summary>
        /// Posts data and gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <param name="paramz">The data to send to the page.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        EAutopayResponse Post(string uri, NameValueCollection paramz);
    }
}
