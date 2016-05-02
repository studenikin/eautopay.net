using System.Collections.Generic;

namespace EAutopay.Forms
{
    public interface IFormRepository
    {
        Form Get(int id);

        List<Form> GetAll();

        Form GetNewest();

        int Save(Form form);

        void Delete(Form form);
    }
}