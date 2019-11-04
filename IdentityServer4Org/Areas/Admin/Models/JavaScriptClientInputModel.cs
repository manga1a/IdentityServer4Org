namespace IdentityServer4Org.Areas.Admin.Models
{
    public class JavaScriptClientInputModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string AllowedCorsOrigin { get; set; }
        public string AllowedScopes { get; set; }
        public bool RequiredConsent { get; set; }

    }
}
