using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class SendPaymentProcess : ISendPaymentProcess
    {
        private readonly ILogger<SendPaymentProcess> _logger;
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public SendPaymentProcess(ILogger<SendPaymentProcess> logger,
                                  IApiGatewayProxy apiGatewayProxy)
        {
            _logger = logger;
            _apiGatewayProxy = apiGatewayProxy;
        }

        public async Task SendPayment(PaymentDto paymentDto)
        {
            await _apiGatewayProxy.MakePayment(paymentDto);
        }
    }
}
