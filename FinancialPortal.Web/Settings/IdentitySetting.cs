namespace FinancialPortal.Web.Settings
{
    public class IdentitySetting
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
        public string ScopeId { get; set; }
        public string ScopeSecret { get; set; }
        public string RegisterEndpoint { get; set; }
        public string ResendEmailConfirmationEmailEndpoint { get; set; }
        public string UpdatePropertiesEndpoint { get; set; }
        public string FindAccountEndpoint { get; set; }
        public string TokenType { get; set; }
        public string AnonymousCredentialUsername { get; set; }
        public string AnonymousCredentialPassword { get; set; }
    }
}