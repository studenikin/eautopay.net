using System.Collections.Generic;

using EAutopay.Forms;

using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    internal class EAutopayFormParser : IFormParser
    {
        /// <summary>
        /// Gets the list of forms on the "forms" page in E-Autopay
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Form"/>.</returns>
        public List<Form> ExtractForms(string source)
        {
            var forms = new List<Form>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var root = htmlDoc.DocumentNode;
            var table = root.SelectSingleNode("//table[@id='table_group_0']");
            if (table != null)
            {
                var rows = table.SelectNodes("tr[@id]");

                if (rows != null)
                {
                    foreach (var tr in rows)
                    {
                        var form = new Form();
                        FillFormDataRow(form, tr);
                        forms.Add(form);
                    }
                }
            }
            return forms;
        }

        private void FillFormDataRow(Form form, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");
            form.ID = int.Parse(tds[0].InnerText.Trim());
            form.Name = tds[2].InnerText.Trim();
        }
    }
}
