namespace EAutopay.Forms
{
    /// <summary>
    /// Represents form in E-Autopay.
    /// </summary>
    public class Form
    {
        /// <summary>
        /// ID of the form.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Name of the form.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns True if the form hasn't been saved in E-Autopay yet. Otherwise - False.
        /// </summary>
        public bool IsNew
        {
            get { return ID == 0;}
            set { ID = 0; }
        }
    }
}
