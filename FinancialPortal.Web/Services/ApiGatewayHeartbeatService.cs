using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class ApiGatewayHeartbeatService : IApiGatewayHeartbeatService
    {
        private readonly ILogger<ApiGatewayHeartbeatService> _logger;
        private readonly IApiGatewayHeartbeatProcess _apiGatewayHeartbeatProcess;

        public ApiGatewayHeartbeatService(ILogger<ApiGatewayHeartbeatService> logger, 
                                          IApiGatewayHeartbeatProcess apiGatewayHeartbeatProcess)
        {
            _logger = logger;
            _apiGatewayHeartbeatProcess = apiGatewayHeartbeatProcess;
        }

        public async Task<HeartBeatDto> CallHeartbeatAsync()
        {
            HeartBeatDto gatewayApiResult;

            try
            {
                gatewayApiResult = await _apiGatewayHeartbeatProcess.CheckApiGatewayHeartbeat();
            }
            catch
            {
                gatewayApiResult = new HeartBeatDto
                {
                    ServiceName = "Financial Gateway API",
                    Status = "RED",
                    Details = "Cannot call Financial Gateway API from Financial Portal"
                };
            }

            return gatewayApiResult;
        }
    }
}