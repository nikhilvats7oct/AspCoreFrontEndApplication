using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class ApiGatewayHeartbeatProcess : IApiGatewayHeartbeatProcess
    {
        private readonly ILogger<ApiGatewayHeartbeatProcess> _logger;
        private readonly IApiGatewayProxy _financialApiGatewayProxy;

        public ApiGatewayHeartbeatProcess(ILogger<ApiGatewayHeartbeatProcess> logger,
                                          IApiGatewayProxy financialApiGatewayProxy)
        {
            _logger = logger;
            _financialApiGatewayProxy = financialApiGatewayProxy;
        }

        public async Task<HeartBeatDto> CheckApiGatewayHeartbeat()
        {
            return await _financialApiGatewayProxy.CheckAllServices();
        }
    }
}