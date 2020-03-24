using FinancialPortal.Web.Services.Interfaces.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    public class DummyController : BaseController
    {
        public DummyController(ILogger<BaseController> logger,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState, 
            IConfiguration configuration) 
            : base(logger, distributedCache, sessionState, configuration)
        {
        }
    }
}