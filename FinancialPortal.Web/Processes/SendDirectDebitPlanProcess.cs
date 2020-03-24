using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class SendDirectDebitPlanProcess : ISendDirectDebitPlanProcess
    {
        private readonly ILogger<SendDirectDebitPlanProcess> _logger;
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public SendDirectDebitPlanProcess(ILogger<SendDirectDebitPlanProcess> logger,
                                          IApiGatewayProxy apiGatewayProxy)
        {
            _logger = logger;
            _apiGatewayProxy = apiGatewayProxy;
        }

        public async Task SendDirectDebitPlanAsync(DirectDebitPaymentDto dto)
        {
            await _apiGatewayProxy.SetupDirectDebitPlanAsync(dto);
        }

    }
}
