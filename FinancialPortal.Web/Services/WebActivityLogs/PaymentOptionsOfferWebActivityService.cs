using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService
    {
        public async Task LogDirectDebitMyOfferOptionSelectedPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteFailedPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledPositiveIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE);
        }

        public async Task LogDirectDebitMyOfferOptionSelectedNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteFailedNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledNegativeIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE);
        }

        public async Task LogDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DirectDebitPaymentMyOfferOptionSelectedWithNoIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE);
        }

        public async Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId,
                WebActionType.DirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE);
        }
    }
}
