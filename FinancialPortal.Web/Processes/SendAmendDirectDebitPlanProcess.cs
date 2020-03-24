using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class SendAmendDirectDebitPlanProcess : ISendAmendDirectDebitPlanProcess
    {
        private readonly ILogger<SendAmendDirectDebitPlanProcess> _logger;
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public SendAmendDirectDebitPlanProcess(ILogger<SendAmendDirectDebitPlanProcess> logger,
                                          IApiGatewayProxy apiGatewayProxy)
        {
            _logger = logger;
            _apiGatewayProxy = apiGatewayProxy;

        }

        public async Task SendAmendDirectDebitPlanAsync(DirectDebitPaymentDto directDebitPaymentDto)
        {
            await _apiGatewayProxy.AmendDirectDebitPlanAsync(directDebitPaymentDto);
        }
    }
}