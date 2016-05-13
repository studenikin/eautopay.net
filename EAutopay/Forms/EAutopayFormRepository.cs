using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

using EAutopay.Parsers;

namespace EAutopay.Forms
{
    /// <summary>
    /// Provides CRUD operations for forms in E-Autopay.
    /// </summary>
    public class EAutopayFormRepository : IFormRepository
    {
        readonly IConfiguration _config;

        readonly ICrawler _crawler;

        readonly IFormParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayFormRepository"/> class.
        /// </summary>
        public EAutopayFormRepository() : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayFormRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        /// <param name="crawler"><see cref="ICrawler"/> to make HTTP requests to E-Autopay.</param>
        /// <param name="parser"><see cref="IFormParser"/> to parse response delivered by the crawler.</param>
        public EAutopayFormRepository(IConfiguration config, ICrawler crawler, IFormParser parser)
        {
            _config = config ?? new AppConfig();
            _crawler = crawler ?? new Crawler();
            _parser = parser ?? new EAutopayFormParser();
        }

        /// <summary>
        /// Gets the form in E-Autopay for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the form.</param>
        /// <returns>A <see cref="Form"/></returns>
        public Form Get(int id)
        {
            return GetAll()
                .Where(f => f.ID == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets all forms in E-Autopay.
        /// </summary>
        /// <returns>The list of <see cref="Form"/>.</returns>
        public List<Form> GetAll()
        {
            var up = new UriProvider(_config.Login);
            var resp = _crawler.Get(up.FormListUri, null);
            return _parser.ExtractForms(resp.Data);
        }

        /// <summary>
        /// Returns latest created form in E-Autopay.
        /// </summary>
        /// <returns>A <see cref="Form"/></returns>
        public Form GetNewest() 
        {
            return GetAll()
                .OrderByDescending(f => f.ID)
                .FirstOrDefault();
        }

        /// <summary>
        /// Creates a new form in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be updated/created.</param>
        /// <returns><see cref="Form"/> ID.</returns>
        public int Save(Form form)
        {
            SaveInEAutopay(form);

            if (form.IsNew)
            {
                form.ID = GetLatestFormID();
            }
            return form.ID;
        }

        /// <summary>
        /// Deletes the specified form from E-Autopay.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be deleted.</param>
        public void Delete(Form form)
        {
            if (form.IsNew) return;

            DeleteFromEAutopay(form);

            ResetFormValues(form);
        }

        private void ResetFormValues(Form form)
        {
            form.ID = 0;
            form.Name = string.Empty;
        }

        private int GetLatestFormID()
        {
            var latest = GetAll()
                .OrderByDescending(f => f.ID)
                .FirstOrDefault();

            return latest != null ? latest.ID : 0;
        }

        private void SaveInEAutopay(Form form)
        {
            var paramz = new NameValueCollection
            {
                {"action", "new"},
                {"extra_settings", "{}"},
                {"name_form", form.Name}
            };

            var up = new UriProvider(_config.Login);
            _crawler.Post(up.FormSaveUri, paramz);
        }

        private void DeleteFromEAutopay(Form form)
        {
            var paramz = new NameValueCollection
            {
                {"id", form.ID.ToString()}
            };

            var up = new UriProvider(_config.Login);
            _crawler.Post(up.FormDeleteUri, paramz);
        }
    }
}
