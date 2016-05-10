using System.Collections.Generic;

using EAutopay.Forms;

namespace EAutopay.Parsers
{
    public interface IFormParser
    {
        List<Form> ExtractForms(string source);
    }
}
