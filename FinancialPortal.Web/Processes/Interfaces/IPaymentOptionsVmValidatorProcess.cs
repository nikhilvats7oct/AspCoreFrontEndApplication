using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IPaymentOptionsVmValidatorProcess
    {
        bool Validate(PaymentOptionsVm paymentOptionsVm);
    }
}
