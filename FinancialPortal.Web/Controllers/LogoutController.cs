using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class LogoutController : BaseController
    {
        public LogoutController(ILogger<BaseController> logger,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IConfiguration configuration)
            : base(logger, distributedCache, sessionState, configuration)
        {
        }

        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();

                if (HttpContext.Request.Cookies.Keys.Contains(".AspNetCore.Session"))
                {
                    HttpContext.Response.Cookies.Delete(".AspNetCore.Session");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "An unknown error has occurred while clearing session.");
            }

            return SignOut(
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}