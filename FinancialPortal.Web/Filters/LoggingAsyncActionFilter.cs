using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Filters
{
    public class LoggingAsyncActionFilter : IAsyncActionFilter
    {
        protected ILogger<LoggingAsyncActionFilter> _logger { get; }
        public LoggingAsyncActionFilter(ILogger<LoggingAsyncActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            _logger.LogInformation($"{context.HttpContext.Request.Protocol} {context.HttpContext.Request.Method} {context.HttpContext.Request.Path} starting.");

            await next();

        }
    }
}
