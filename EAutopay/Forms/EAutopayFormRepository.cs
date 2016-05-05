using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace EAutopay.Forms
{
    /// <summary>
    /// Provides CRUD operations for Form entity.
    /// </summary>
    public class EAutopayFormRepository : IFormRepository
    {
        readonly IConfiguration _config;

        public EAutopayFormRepository() : this(null)
        { }

        public EAutopayFormRepository(IConfiguration config)
        {
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Returns form object with specified ID.
        /// </summary>
        /// <param name="id">ID of the form to be returned.</param>
        /// <returns>Form with specified ID.</returns>
        public Form Get(int id)
        {
            var forms = GetAll();
            return forms.Where(f => f.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns all forms existing in E-Autopay.
        /// </summary>
        /// <returns>List of forms.</returns>
        public List<Form> GetAll()
        {
            var forms = new List<Form>();

            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.FormListUri))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());
                return parser.GetForms();
            }
        }

        /// <summary>
        /// Returns latest created form.
        /// </summary>
        /// <returns>Form object.</returns>
        public Form GetNewest() 
        {
            var forms = GetAll();
            return forms.OrderByDescending(f => f.ID).FirstOrDefault();
        }

        /// <summary>
        /// Creates new form in E-Autopay. Or updates existing one.
        /// </summary>
        /// <param name="form">Form to be updated/created.</param>
        /// <returns>Form ID.</returns>
        public int Save(Form form)
        {
            var paramz = new NameValueCollection
            {
                {"action", "new"},
                {"extra_settings", "{}"},
                {"name_form", form.Name}
            };

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(_config.FormSaveUri, paramz))
            {
                if (form.IsNew)
                {
                    var lastForm = GetNewest();
                    if (lastForm != null)
                    {
                        form.ID = lastForm.ID;
                    }
                }
                return form.ID;
            }
        }

        /// <summary>
        /// Deletes specified form in E-Autopay.
        /// </summary>
        /// <param name="form">Form to be deleted.</param>
        public void Delete(Form form)
        {
            if (form.IsNew) return;

            var paramz = new NameValueCollection
            {
                {"id", form.ID.ToString()}
            };

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(_config.FormDeleteUri, paramz))
            {
                ResetFormValues(form);
            }
        }

        private void ResetFormValues(Form form)
        {
            form.ID = 0;
            form.Name = string.Empty;
        }
    }

}
