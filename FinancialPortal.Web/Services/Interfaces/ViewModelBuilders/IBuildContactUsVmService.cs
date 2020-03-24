using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces.ViewModelBuilders
{
    public interface IBuildContactUsVmService
    {
        ContactUsVm CreateNewContactUsVm();

        ContactUsVm ReinstateContactUsVm(ContactUsVm model);
    }
}
