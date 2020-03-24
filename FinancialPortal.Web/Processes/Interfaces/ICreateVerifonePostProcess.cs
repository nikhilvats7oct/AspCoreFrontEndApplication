using FinancialPortal.Web.Models.Verifone;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface ICreateVerifonePostProcess
    {
        PostDataModel CreateDataModel(string request);
    }
}
