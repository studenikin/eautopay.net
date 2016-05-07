namespace EAutopay.Security
{
    /// <summary>
    /// Authentication results supported by E-Autopay.
    /// </summary>
    public enum AuthStatus
    {
        /// <summary>
        /// Successful authentication.
        /// </summary>
        Ok = 1,
        /// <summary>
        /// Incorrect login or password.
        /// </summary>
        Login_Failed = 2,
        /// <summary>
        /// Need answer to secret question.
        /// </summary>
        Need_Secret = 3,
        /// <summary>
        /// Incorrect secret answer.
        /// </summary>
        Secret_Failed = 4
    }
}
