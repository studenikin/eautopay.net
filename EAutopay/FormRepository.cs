using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace EAutopay
{
    /// <summary>
    /// Provides CRUD operations for Form entity.
    /// </summary>
    public class FormRepository
    {
        private FormRepository() { }

        /// <summary>
        /// Returns form object with specified ID.
        /// </summary>
        /// <param name="id">ID of the form to be returned.</param>
        /// <returns>Form with specified ID.</returns>
        public static Form Get(int id)
        {
            var forms = GetAll();
            return forms.Where(f => f.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns all forms existing in E-Autopay.
        /// </summary>
        /// <returns>List of forms.</returns>
        public static List<Form> GetAll()
        {
            var forms = new List<Form>();

            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.URI_FORM_LIST))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());

                foreach (var dr in parser.GetFormDataRows())
                {
                    var f = new Form();
                    f.Fill(dr);
                    forms.Add(f);
                }
            }
            return forms;
        }

        /// <summary>
        /// Returns latest created form.
        /// </summary>
        /// <returns>Form object.</returns>
        public static Form GetNewest() 
        {
            var forms = GetAll();
            return forms.OrderByDescending(f => f.ID).FirstOrDefault();
        }
    }

}
