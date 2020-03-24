using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Filters
{
    public class ExceptionLoggerFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionLoggerFilter> _logger;

        public ExceptionLoggerFilter(ILogger<ExceptionLoggerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhanled exception has occurred.");

            base.OnException(context);
        }
    }
}
