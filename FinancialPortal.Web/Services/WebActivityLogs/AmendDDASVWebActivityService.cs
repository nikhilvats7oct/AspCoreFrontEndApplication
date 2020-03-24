using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogAmendDirectDebitASVOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentASVOptionSelected);
        }

        public async Task LogAmendDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentASVOptionDetailsEntered);
        }

        public async Task LogAmendDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentASVOptionCompleteSuccess);
        }

        public async Task LogAmendDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentASVOptionCompleteFailure);
        }

        public async Task LogAmendDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentASVOptionCompleteCancelled);
        }
    }
}
