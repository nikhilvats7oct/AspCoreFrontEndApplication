using System.Threading.Tasks;
using FinancialPortal.Web.Middleware;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(ILogger<BaseController> logger,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IConfiguration configuration)
            : base(logger, distributedCache, sessionState, configuration)
        {
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [Route("error")]
        public IActionResult Error()
        {
            return DefaultError();
        }

        [Route("errorcode")]
        public IActionResult ErrorCode(string code)
        {
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [Route("error/authfailure")]
        public IActionResult AuthFailure()
        {
            return DefaultError();
        }

        private IActionResult DefaultError()
        {
            var traceId = Request.Headers.ContainsKey(TraceMiddleware.TraceHeaderName)
               ? Request.Headers[TraceMiddleware.TraceHeaderName].ToString()
               : "";

            // TODO: Not sure if we need to log here.

            return View("Error", new ErrorViewModel
            {
                ExceptionOccurrenceId = traceId,
                ShowOccurrenceInformation = true
            });
        }
    }
}