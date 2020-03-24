using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces.ViewModelBuilders
{
    public interface IBuildOneOffPaymentReviewVmService
    {
        Task<IBuildOneOffPaymentReviewVmValidationResult> ValidateAndBuild(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState,
            Guid lowellReferenceSurrogateKey, PaymentOptionsVm paymentOptionsVmWithUserEntries, string caseflowUserId);
    }
}
