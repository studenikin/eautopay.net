using System;
using System.Collections.Specialized;

namespace EAutopay.NET
{
    public class Form
    {
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
        }

        public string Name { get; set; }

        private bool IsNew
        {
            get
            {
                return _id == 0;
            }
            set
            {
                _id = 0;
            }
        }

        public int Save()
        {
            var paramz = new NameValueCollection
            {
                {"action", "new"},
                {"extra_settings", "{}"},
                {"name_form", Name}
            };

            var poster = new Poster();
            poster.HttpPost(Config.URI_FORM_SAVE, paramz);

            if (IsNew)
            {
                var lastForm = FormRepository.GetNewest();
                if (lastForm != null)
                {
                    _id = lastForm.ID;
                }
            }
            return _id;
        }

        public void Delete()
        {
            if (IsNew) return;

            var paramz = new NameValueCollection
            {
                {"id", ID.ToString()}
            };

            var poster = new Poster();
            poster.HttpPost(Config.URI_FORM_DELETE, paramz);

            IsNew = true;
        }

        internal void Fill(IFormDataRow dr)
        {
            _id = dr.ID;
            Name = dr.Name;
        }
    }
}
