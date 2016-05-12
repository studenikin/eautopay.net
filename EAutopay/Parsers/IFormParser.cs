using System.Collections.Generic;

using EAutopay.Forms;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Extracts the form data out of the scpecific source.
    /// </summary>
    public interface IFormParser
    {
        /// <summary>
        /// Gets the list of forms in E-Autopay
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Form"/>.</returns>
        List<Form> ExtractForms(string source);
    }
}
