using System.Configuration;

namespace EAutopay
{
    /// <summary>
    /// Encapsulates exception that is thrown if configuration file missing required element(s).
    /// </summary>
    public class ConfigurationMissingException : ConfigurationErrorsException
    {
        /// <summary>
        /// Name of the missing element in the configuration file.
        /// </summary>
        public string MissingElement { get; private set; }

        /// <summary>
        /// Initializes instance of the object.
        /// </summary>
        /// <param name="missingElement">Missing element name.</param>
        public ConfigurationMissingException(string missingElement) : base(missingElement)
        {
            MissingElement = missingElement;
        }
    }
}
