using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces.ViewModelBuilders
{
    public interface IBuildDirectDebitDetailsVmService
    {
        DirectDebitDetailsVm Build(PaymentOptionsVm paymentOptionsVm);

        void UpdateFieldsFromUserEntries(DirectDebitDetailsVm paymentOptionsToUpdate, DirectDebitDetailsVm paymentOptionsUserEntries);
    }
}
