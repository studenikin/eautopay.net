using System.Configuration;

namespace EAutopay
{
    /// <summary>
    /// Encapsulates exception that is thrown if configuration file missing required element(s).
    /// </summary>
    public class ConfigurationMissingException : ConfigurationErrorsException
    {
        /// <summary>
        /// The name of the missing element in the configuration file.
        /// </summary>
        public string MissingElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
        /// </summary>
        /// <param name="missingElement">The name of the missing element.</param>
        public ConfigurationMissingException(string missingElement) : base(missingElement)
        {
            MissingElement = missingElement;
        }
    }
}
