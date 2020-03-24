using FinancialPortal.Web.Models.Verifone;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface ICreateVerifoneRequestProcess
    {
        RequestDataModel CreateDataModel(PaymentOptionsVm accountVm);
    }
}
