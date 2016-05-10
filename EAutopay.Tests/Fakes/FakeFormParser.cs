using System.Collections.Generic;

using EAutopay.Forms;
using EAutopay.Parsers;

namespace EAutopay.Tests.Fakes
{
    public class FakeFormParser : IFormParser
    {
        public List<Form> ExtractForms(string source)
        {
            var ret = new List<Form>();

            var f1 = new Form();
            f1.ID = 1;
            f1.Name = "form 1";

            var f2 = new Form();
            f2.ID = 2;
            f2.Name = "form 2";

            ret.Add(f1);
            ret.Add(f2);

            return ret;
        }
    }
}
