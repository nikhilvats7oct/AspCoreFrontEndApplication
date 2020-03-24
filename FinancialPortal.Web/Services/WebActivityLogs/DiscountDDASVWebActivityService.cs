using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDiscountDirectDebitASVOptionSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DiscountDirectDebitPaymentASVOptionSelected);
        }

        public async Task LogDiscountDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DiscountDirectDebitPaymentASVOptionDetailsEntered);
        }

        public async Task LogDiscountDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DiscountDirectDebitPaymentASVOptionCompleteSuccess);
        }

        public async Task LogDiscountDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DiscountDirectDebitPaymentASVOptionCompleteFailure);
        }

        public async Task LogDiscountDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DiscountDirectDebitPaymentASVOptionCompleteCancelled);
        }
    }
}
