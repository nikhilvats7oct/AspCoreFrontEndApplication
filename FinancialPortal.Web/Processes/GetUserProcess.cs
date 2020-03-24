using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class GetUserProcess : IGetUserProcess
    {
        private readonly ILogger<GetUserProcess> _logger;
        private readonly IApiGatewayProxy _financialApiGatewayProxy;

        public GetUserProcess(ILogger<GetUserProcess> logger,
                              IApiGatewayProxy financialApiGatewayProxy)
        {
            _logger = logger;
            _financialApiGatewayProxy = financialApiGatewayProxy;
        }

        public async Task<UserDto> GetUser(string userId)
        {
            return await _financialApiGatewayProxy.GetUser(userId);
        }
    }
}
