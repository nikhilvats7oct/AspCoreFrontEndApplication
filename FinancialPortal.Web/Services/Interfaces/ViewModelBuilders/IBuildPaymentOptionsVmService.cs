using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces.ViewModelBuilders
{
    public interface IBuildPaymentOptionsVmService
    {
        Task<PaymentOptionsVm> Build(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState, 
            Guid lowellReferenceSurrogateKey, string caseflowUserId);

        void UpdateFieldsFromUserEntries(
            IUserIdentity loggedInUser,
            PaymentOptionsVm paymentOptionsToUpdate, PaymentOptionsVm paymentOptionsUserEntries);
    }
}
