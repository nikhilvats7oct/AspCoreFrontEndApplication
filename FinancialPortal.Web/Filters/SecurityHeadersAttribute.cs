using FinancialPortal.Web.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinancialPortal.Web.Filters
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        private readonly ContentSecurityPolicyHeaderSetting _cspSetting;

        public SecurityHeadersAttribute(ContentSecurityPolicyHeaderSetting cspSetting)
        {
            _cspSetting = cspSetting;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                var csp = string.Join("", _cspSetting.Policies);
                
                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
                }

                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                var refererPolicy = "no-referrer";
                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", refererPolicy);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
                var protection = "1; mode=block";
                if (!context.HttpContext.Response.Headers.ContainsKey("X-XSS-Protection"))
                {
                    context.HttpContext.Response.Headers.Add("X-XSS-Protection", protection);
                }
            }
        }
    }
}