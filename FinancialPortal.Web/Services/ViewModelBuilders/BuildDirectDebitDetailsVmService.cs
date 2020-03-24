using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildDirectDebitDetailsVmService : IBuildDirectDebitDetailsVmService
    {
        public DirectDebitDetailsVm Build(PaymentOptionsVm paymentOptionsVm)
        {
            return new DirectDebitDetailsVm()
            {
                LowellRef = paymentOptionsVm.LowellReference,
                DiscountAvailable = paymentOptionsVm.DiscountedBalancePreviouslyAccepted || paymentOptionsVm.DiscountBalanceAvailable,
                PaymentAmount = paymentOptionsVm.DirectDebitAmount,
                PaymentFrequency = paymentOptionsVm.DirectDebitSelectedFrequency,
                DiscountSelected = paymentOptionsVm.DiscountAccepted,
                SelectedPlanSetupOption = paymentOptionsVm.SelectedPlanSetupOption
            };
        }

        public void UpdateFieldsFromUserEntries(DirectDebitDetailsVm paymentOptionsToUpdate, DirectDebitDetailsVm paymentOptionsUserEntries)
        {
            paymentOptionsToUpdate.AcceptDirectDebitGuarantee = paymentOptionsUserEntries.AcceptDirectDebitGuarantee;
            paymentOptionsToUpdate.AccountHoldersName = paymentOptionsUserEntries.AccountHoldersName;
            paymentOptionsToUpdate.AccountNumber = paymentOptionsUserEntries.AccountNumber;
            paymentOptionsToUpdate.SortCode = paymentOptionsUserEntries.SortCode;
        }

    }
}
