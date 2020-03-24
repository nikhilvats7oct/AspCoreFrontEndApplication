using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogAmendDirectDebitDIOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentDIOptionSelected);
        }

        public async Task LogAmendDirectDebitDIOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentDIOptionDetailsEntered);
        }

        public async Task LogAmendDirectDebitDIOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentDIOptionCompleteSuccess);
        }

        public async Task LogAmendDirectDebitDIOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentDIOptionCompleteFailure);
        }

        public async Task LogAmendDirectDebitDIOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentDIOptionCompleteCancelled);
        }
    }
}
