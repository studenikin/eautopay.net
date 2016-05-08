using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace EAutopay.Forms
{
    /// <summary>
    /// Provides CRUD operations for forms in E-Autopay.
    /// </summary>
    public class EAutopayFormRepository : IFormRepository
    {
        readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayFormRepository"/> class.
        /// </summary>
        public EAutopayFormRepository() : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayFormRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        public EAutopayFormRepository(IConfiguration config)
        {
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Gets the form in E-Autopay for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the form.</param>
        /// <returns>A <see cref="Form"/></returns>
        public Form Get(int id)
        {
            var forms = GetAll();
            return forms.Where(f => f.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Gets all forms in E-Autopay.
        /// </summary>
        /// <returns>The list of <see cref="Form"/>.</returns>
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
        /// Returns latest created form in E-Autopay.
        /// </summary>
        /// <returns>A <see cref="Form"/></returns>
        public Form GetNewest() 
        {
            var forms = GetAll();
            return forms.OrderByDescending(f => f.ID).FirstOrDefault();
        }

        /// <summary>
        /// Creates a new form in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be updated/created.</param>
        /// <returns><see cref="Form"/> ID.</returns>
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
        /// Deletes the specified form from E-Autopay.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be deleted.</param>
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
