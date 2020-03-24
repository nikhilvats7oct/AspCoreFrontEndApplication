using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IBuildPaymentDtoProcess _buildPaymentDtoProcess;
        private readonly ISendPaymentProcess _sendPaymentProcss;

        public PaymentService(ILogger<PaymentService> logger,
                              IBuildPaymentDtoProcess buildPaymentDtoProcess,
                              ISendPaymentProcess sendPaymentProcss)
        {
            _logger = logger;
            _buildPaymentDtoProcess = buildPaymentDtoProcess;
            _sendPaymentProcss = sendPaymentProcss;
        }

        public async Task MakePayment(PaymentResultVm model, OneOffPaymentDto oneOffPaymentDto)
        {
            var dto = _buildPaymentDtoProcess.BuildPaymentDto(model, oneOffPaymentDto);
            await _sendPaymentProcss.SendPayment(dto);
        }
    }
}
