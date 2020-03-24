using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDirectDebitASVOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentASVOptionSelected);
        }

        public async Task LogDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentASVOptionDetailsEntered);
        }

        public async Task LogDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentASVOptionCompleteSuccess);
        }

        public async Task LogDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentASVOptionCompleteFailure);
        }

        public async Task LogDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentASVOptionCompleteCancelled);
        }
    }
}
