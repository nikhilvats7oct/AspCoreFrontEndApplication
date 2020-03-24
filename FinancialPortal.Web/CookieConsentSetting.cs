using System;
using Newtonsoft.Json;

namespace FinancialPortal.Web
{
    public class CookieConsentSetting
    {
        public DateTime Date { get; set; }

        /// <summary>
        ///     Essential. Must always be true for the site to function.
        /// </summary>
        public bool Essential => true;

        /// <summary>
        ///     Functional cookie. Reserved for future use.
        /// </summary>
        [JsonProperty("performance")]
        public bool Performance { get; set; }

        /// <summary>
        ///     Third Party. Eg: GA Cookies.
        /// </summary>
        [JsonProperty("marketing")]
        public bool Marketing { get; set; }
    }
}