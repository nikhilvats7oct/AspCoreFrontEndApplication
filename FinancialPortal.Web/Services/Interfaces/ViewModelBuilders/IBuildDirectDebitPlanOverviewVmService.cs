using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces.ViewModelBuilders
{
    public interface IBuildDirectDebitPlanOverviewVmService
    {
        DirectDebitPlanOverviewVm Build(PaymentOptionsVm accountVm, DirectDebitDetailsVm directDebitDetailsVm);

        Task<IBuildDirectDebitPlanOverviewVmValidationResult> ValidateAndBuild(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState, Guid lowellReferenceSurrogateKey,
            PaymentOptionsVm paymentOptionsVmWithUserEntries, DirectDebitDetailsVm directDebitDetailsVmWithUserEntries, string caseflowUserId);
    }
}
