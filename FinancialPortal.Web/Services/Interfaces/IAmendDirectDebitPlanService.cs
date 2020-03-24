using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IAmendDirectDebitPlanService
    {
        Task<bool> AmendDirectDebitPlan(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState,
            Guid lowellReferenceSurrogateKey, AmendDirectDebitVm amendDirectDebitVmWithUserEntries,string caseflowUserId);

        int CalculateTermInMonths(decimal balance, decimal paymentAmount, string paymentFrequency);
    }
}