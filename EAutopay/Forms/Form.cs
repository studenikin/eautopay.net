namespace EAutopay.Forms
{
    public class Form
    {
        IFormRepository _repository;

        public int ID { get; internal set; }

        public string Name { get; set; }

        internal bool IsNew
        {
            get { return ID == 0;}
            set { ID = 0; }
        }

        public Form()
        {
            _repository = new EAutopayFormRepository();
        }

        public Form(IFormRepository repository)
        {
            _repository = repository;
        }

        public int Save()
        {
            return _repository.Save(this);
        }

        public void Delete()
        {
            if (IsNew) return;
            _repository.Delete(this);
            IsNew = true;
        }

        internal void Fill(IFormDataRow dr)
        {
            ID = dr.ID;
            Name = dr.Name;
        }
    }
}
