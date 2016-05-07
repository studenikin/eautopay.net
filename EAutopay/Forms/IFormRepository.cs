using System.Collections.Generic;

namespace EAutopay.Forms
{
    /// <summary>
    /// Provides CRUD operations for forms in E-Autopay.
    /// </summary>
    public interface IFormRepository
    {
        /// <summary>
        /// Gets the form in E-Autopay for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the form.</param>
        /// <returns>A <see cref="Form"/></returns>
        Form Get(int id);

        /// <summary>
        /// Gets all forms in E-Autopay.
        /// </summary>
        /// <returns>The list of <see cref="Form"/>.</returns>
        List<Form> GetAll();

        /// <summary>
        /// Returns latest created form in E-Autopay.
        /// </summary>
        /// <returns>A <see cref="Form"/></returns>
        Form GetNewest();

        /// <summary>
        /// Creates a new form in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be updated/created.</param>
        /// <returns>Form ID.</returns>
        int Save(Form form);

        /// <summary>
        /// Deletes the specified form in E-Autopay.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be deleted.</param>
        void Delete(Form form);
    }
}