using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogAmendDirectDebitMyOfferOptionSelectedWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AmendDirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(string lowellRef,
            string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionSelectedWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionSelectedWithNoIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE);
        }

        public async Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE);
        }
    }
}
