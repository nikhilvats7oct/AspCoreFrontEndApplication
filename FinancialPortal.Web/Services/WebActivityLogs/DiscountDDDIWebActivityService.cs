using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDiscountDirectDebitDIOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentDIOptionSelected);
        }

        public async Task LogDiscountDirectDebitDIOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentDIOptionDetailsEntered);
        }

        public async Task LogDiscountDirectDebitDIOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentDIOptionCompleteSuccess);
        }

        public async Task LogDiscountDirectDebitDIOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentDIOptionCompleteFailure);
        }

        public async Task LogDiscountDirectDebitDIOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentDIOptionCompleteCancelled);
        }
    }
}
