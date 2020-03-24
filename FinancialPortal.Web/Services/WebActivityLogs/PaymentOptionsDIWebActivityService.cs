using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDirectDebitPaymentDIOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentDIOptionSelected);
        }

        public async Task LogDirectDebitPaymentDIOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentDIOptionDetailsEntered);
        }

        public async Task LogDirectDebitPaymentDIOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentDIOptionCompleteSuccess);
        }

        public async Task LogDirectDebitPaymentDIOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentDIOptionCompleteFailure);
        }

        public async Task LogDirectDebitPaymentDIOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentDIOptionCompleteCancelled);
        }
    }
}
