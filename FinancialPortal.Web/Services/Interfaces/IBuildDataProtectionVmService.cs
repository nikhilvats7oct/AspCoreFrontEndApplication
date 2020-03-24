using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IBuildDataProtectionVmService
    {
        DataProtectionVm Build();

        void PopulateDateComponents(DataProtectionVm existingVm);
    }
}
