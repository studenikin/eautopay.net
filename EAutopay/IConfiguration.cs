namespace EAutopay
{
    /// <summary>
    /// Encapsulates general E-Autopay settings.
    /// </summary>
    public interface  IConfiguration
    {
        /// <summary>
        /// Gets URI of the login page.
        /// </summary>
        string LoginUri { get; }

        /// <summary>
        /// Gets URI of the logout page.
        /// </summary>
        string LogoutUri { get; }

        /// <summary>
        /// Gets URI of the secret page. Where secret answer is provided.
        /// </summary>
        string SecretUri { get; }

        /// <summary>
        /// Gets URI of the main page. Where user is redirected after logging in.
        /// </summary>
        string MainUri { get; }

        /// <summary>
        /// Gets URI of the product list page.
        /// </summary>
        string ProductListUri { get; }

        /// <summary>
        /// Gets URI of the product save page.
        /// </summary>
        string ProductSaveUri { get; }

        /// <summary>
        /// Gets URI of the product delete page.
        /// </summary>
        string ProductDeleteUri { get; }

        /// <summary>
        /// Gets URI of the form list page.
        /// </summary>
        string FormListUri { get; }

        /// <summary>
        /// Gets URI of the form save page.
        /// </summary>
        string FormSaveUri { get; }

        /// <summary>
        /// Gets URI of the form delete page.
        /// </summary>
        string FormDeleteUri { get; }

        /// <summary>
        /// Gets URI of the upsell page for specified product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="upsellId">Upsell ID.</param>
        /// <returns></returns>
        string GetUpsellUri(int productId, int upsellId);

        /// <summary>
        /// Gets URI of the send settings page for specified product.
        /// </summary>
        /// <param name="productId">ID of the product to get settings for.</param>
        /// <returns></returns>
        string GetSendSettingsUri(int productId);

        /// <summary>
        /// Gets URI of the landing page for upsell.
        /// </summary>
        string UpsellLandingPage { get; }

        /// <summary>
        /// Gets interval of time (in minutes) while upsell offer is available.
        /// </summary>
        int UpsellInterval { get; }
    }
}
