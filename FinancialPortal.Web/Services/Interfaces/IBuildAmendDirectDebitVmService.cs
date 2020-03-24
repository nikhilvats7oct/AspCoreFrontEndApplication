using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IBuildAmendDirectDebitVmService
    {
        Task<AmendDirectDebitVm> Build(IApplicationSessionState session, Guid lowellReferenceSurrogateKey,string caseflowUserId);

        void UpdateFieldsFromUserEntries(AmendDirectDebitVm amendDirectDebitToUpdate, AmendDirectDebitVm amendDirectDebitUserEntries);
    }
}