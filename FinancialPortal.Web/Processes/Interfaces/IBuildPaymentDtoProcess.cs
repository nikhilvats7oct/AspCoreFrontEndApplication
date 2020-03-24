using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IBuildPaymentDtoProcess
    {
        PaymentDto BuildPaymentDto(PaymentResultVm model, OneOffPaymentDto oneOffPaymentDt);
    }
}
