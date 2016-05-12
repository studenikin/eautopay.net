namespace EAutopay.Parsers
{
    /// <summary>
    /// Provides secure token over HTTPS.
    /// The token needs to be sent for every request to E-Autopay.
    /// </summary>
    public interface ITokenParser
    {
        /// <summary>
        /// Gets the token from the source.
        /// </summary>
        /// <param name="source">HTML to be parsed.</param>
        /// <returns>The token as string.</returns>
        string ExtractToken(string source);
    }
}
