namespace FinancialPortal.Web.Security
{
    public class OpenIdAuthOptions
    {
        public OpenIdAuthOptions()
        {
            RequireHttpsMetadata = true;
        }

        public string AuthorityEndpoint { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
        public string ScopeId { get; set; }
        public string ScopeSecret { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string RedisConfiguration { get; set; }
        public string TokenType { get; set; }
    }
}