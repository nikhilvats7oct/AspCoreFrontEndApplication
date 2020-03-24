using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDiscountDirectDebitMyOfferOptionSelectedWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionSelectedWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionSelectedWithNoIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE);
        }

        public async Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE);
        }
    }
}
