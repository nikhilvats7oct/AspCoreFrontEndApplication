using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services.WebActivityLogs
{
    public partial class WebActivityService : IWebActivityService
    {
        private readonly ILogger<WebActivityService> _logger;
        private readonly ISendToRabbitMQProcess _sendToRabbitMQProcess;

        public WebActivityService(ILogger<WebActivityService> logger,
                                  ISendToRabbitMQProcess webActionLoggingProcess)
        {
            _logger = logger;
            _sendToRabbitMQProcess = webActionLoggingProcess;
        }

        protected async Task LogWebAction(string lowellRef, string userId, WebActionType webActionType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lowellRef) && string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogError($"Failed to send web log  for WebActionId:{(int)webActionType}, as both lowell ref and user id is null or empty.");
                }
                else if (!string.IsNullOrWhiteSpace(userId) && !Guid.TryParse(userId, out var userIdAsGuid))
                {
                    _logger.LogError($"Failed to send web log  for WebActionId:{(int)webActionType}, for lowell ref {lowellRef} as provided GUID for user id is invalid. user id {userId}.");
                }
                else
                {
                    await _sendToRabbitMQProcess.SendToRabbitMQ(new WebActionDto()
                    {
                        Company = 0,
                        DateTime = DateTime.Now,
                        Guid = userId ?? "",
                        LowellReference = lowellRef,
                        WebActionType = webActionType
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in WebActionLoggingService.LogWebAction ({webActionType.ToString()})");
            }
          
        }
        public async Task LogLoginSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.LoginSuccess);
        }
        public async Task LogLoginFail(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.LoginFail);
        }
        public async Task LogDPACheckSuccess(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DPACheckSuccess);
        }
        public async Task LogDPACheckFail(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DPACheckFail);
        }
        public async Task LogRegistrationRequest(string lowellRef, string userId)
        {
            await LogWebActionAnonymousAsync(lowellRef, userId, WebActionType.RegistrationRequest);
        }
        public async Task LogRegistrationActivated(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.RegistrationActivated);
        }
        public async Task LogOneOffPaymentSelected(string lowellRef, string userId, bool partial, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedPaymentSelectOptions);
            }
            else
            {
                if (partial)
                {
                    await LogWebAction(lowellRef, userId, WebActionType.PartialPaymentSelectOptions);
                }
                else
                {
                    await LogWebAction(lowellRef, userId, WebActionType.FullPaymentSelectOptions);
                }
            }
        }
        public async Task LogOneOffPaymentDetailsEntered(string lowellRef, string userId, bool partial, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedPaymentEnterPaymentDetails);
            }
            else
            {
                if (partial)
                {
                    await LogWebAction(lowellRef, userId, WebActionType.PartialPaymentEnterPaymentDetails);
                }
                else
                {
                    await LogWebAction(lowellRef, userId, WebActionType.FullPaymentEnterPaymentDetails);
                }
            }
        }
        public async Task LogOneOffPaymentComplete(string lowellRef, string userId, bool partial, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedPaymentCompleteSuccess);
            }
            else
            {
                if (partial)
                {
                    await LogWebAction(lowellRef, userId, WebActionType.PartialPaymentCompleteSuccess);
                }
                else
                {
                    await LogWebAction(lowellRef, userId, WebActionType.FullPaymentCompleteSuccess);
                }
            }
        }
        public async Task LogOneOffPaymentFailure(string lowellRef, string userId, bool partial, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedPaymentCompleteFailure);
            }
            else
            {
                if (partial)
                {
                    await LogWebAction(lowellRef, userId, WebActionType.PartialPaymentCompleteFailure);
                }
                else
                {
                    await LogWebAction(lowellRef, userId, WebActionType.FullPaymentCompleteFailure);
                }
            }
        }
        public async Task LogOneOffPaymentCancelled(string lowellRef, string userId, bool partial, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedPaymentCompleteCancelled);
            }
            else
            {
                if (partial)
                {
                    await LogWebAction(lowellRef, userId, WebActionType.PartialPaymentCompleteCancelled);
                }
                else
                {
                    await LogWebAction(lowellRef, userId, WebActionType.FullPaymentCompleteCancelled);
                }
            }
        }
        public async Task LogSetUpDDPlanSelected(string lowellRef, string userId, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedDDPlanSelectOptions);
            }
            else
            {

                await LogWebAction(lowellRef, userId, WebActionType.DDPlanSelectOptions);
            }

        }
        public async Task LogSetUpDDPlanDetailsEntered(string lowellRef, string userId, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedDDPlanEnterPaymentDetails);
            }
            else
            {
                await LogWebAction(lowellRef, userId, WebActionType.DDPlanEnterPaymentDetails);
            }
        }
        public async Task LogSetUpDDPlanComplete(string lowellRef, string userId, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedDDPlanCompleteSuccess);
            }
            else
            {
                await LogWebAction(lowellRef, userId, WebActionType.DDPlanCompleteSuccess);
            }
        }
        public async Task LogSetUpDDPlanFailure(string lowellRef, string userId, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedDDPlanCompleteFailure);
            }
            else
            {
                await LogWebAction(lowellRef, userId, WebActionType.DDPlanCompleteFailure);
            }
        }
        public async Task LogSetUpDDPlanCancelled(string lowellRef, string userId, bool discounted)
        {
            if (discounted)
            {
                await LogWebAction(lowellRef, userId, WebActionType.DiscountedDDPlanCompleteCancelled);
            }
            else
            {
                await LogWebAction(lowellRef, userId, WebActionType.DDPlanCompleteCancelled);
            }
        }
        public async Task LogAmendDDPlanSelected(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DDAmendPlanSelectOptions);
        }
        public async Task LogAmendDDPlanDetailsEntered(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DDAmendPlanEnterPaymentDetails);
        }
        public async Task LogAmendDDPlanComplete(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DDAmendPlanCompleteSuccess);
        }
        public async Task LogAmendDDPlanFailure(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DDAmendPlanCompleteFailure);
        }
        public async Task LogAmendDDPlanCancelled(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.DDAmendPlanCompleteCancelled);
        }
        public async Task LogBudgetCalculatorHouseholdDetails(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorHouseholdDetails);
        }
        public async Task LogBudgetCalculatorIncome(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorIncome);
        }
        public async Task LogBudgetCalculatorBillsAndOutgoings1(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorBillsAndOutgoings1);
        }
        public async Task LogBudgetCalculatorExpenditure(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorExpenditure);
        }
        public async Task LogBudgetCalculatorCompleted(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorCompleted);
        }
        public async Task LogBudgetCalculatorReplayed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.BudgetCalculatorReplayed);
        }
        public async Task LogMyAccountsPageViewed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.MyAccountsPageViewed);
        }
        public async Task LogAccountDetailsViewed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AccountDetailsViewed);
        }
        public async Task LogAllTransactionsViewed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.AllTransactionsViewed);
        }
        public async Task LogStatementDownloaded(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.StatementDownloaded);
        }
        public async Task LogMyProfileViewed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.ProfileViewed);
        }
        public async Task LogPasswordChanged(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordChanged);
        }
        public async Task LogPasswordResetRequest(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordResetRequest);
        }
        public async Task LogPasswordResetConfirmed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordResetConfirmed);
        }
        public async Task LogEmailAddressChangeRequest(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.EmailAddressChangeRequest);
        }
        public async Task LogEmailAddressChangeConfirmed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.EmailAddressChangeConfirmed);
        }
        public async Task LogPasswordResetEmailRequested(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordResetEmailRequested);
        }
        public async Task LogPasswordChangeRequest(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordChangeRequest);
        }
        public async Task LogPasswordChangeConfirmed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.PasswordChangeConfirmed);
        }
        public async Task LogEmailAddressUpdateRequest(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.EmailAddressUpdateRequest);
        }
        public async Task LogEmailAddressUpdateConfirmed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.EmailAddressUpdateConfirmed);
        }
        private async Task LogWebActionAnonymousAsync(string lowellReference, string userId, WebActionType webActionType)
        {
            try
            {
                await _sendToRabbitMQProcess.SendToRabbitMQAnonymous(new WebActionDto()
                {
                    Company = 0,
                    DateTime = DateTime.Now,
                    Guid = userId ?? "",
                    LowellReference = lowellReference,
                    WebActionType = webActionType
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in WebActionLoggingService.LogWebAction ({webActionType.ToString()})");
            }

        }

        public async Task LogContactPreferencesPageVist(string lowellReference, string userId)
        {
            await LogWebAction(lowellReference, userId, WebActionType.LFLContactPreferencesPageViewed);
        }
        public async Task LogMobilePhoneUpdatePageVist(string lowellReference, string userId)
        {
            await LogWebAction(lowellReference, userId, WebActionType.LFLMobilePhoneUpdatePageViewed);
        }
        public async Task LogContactPreferencesSMSChange(string lowellReference, string userId, bool newValue, bool oldValue)
        {
            if (newValue != oldValue)
            {
                WebActionType webAction = newValue ? WebActionType.LFLSmsSelected : WebActionType.LFLSmsDeselected;

                await LogWebAction(lowellReference, userId, webAction);
            }
        }

        public async Task LogContactPreferecesEmailChange(string lowellReference, string userId, bool newValue, bool oldValue)
        {
            if (newValue != oldValue)
            {
                WebActionType webAction = newValue ? WebActionType.LFLEmailSelected : WebActionType.LFLEmailDeselected;

                await LogWebAction(lowellReference, userId, webAction);
            }
        }
        public async Task LogMobilePhoneNumberUpdateChange(string lowellReference, string userId, string newValue, string oldValue)
        {
            if (newValue != oldValue)
            {
                await LogWebAction(lowellReference, userId, WebActionType.LFLMobilePhoneNumberUpdated);
            }
        }

        public async Task LogViewLFLLetter(string lowellReference, string userId)
        {
            await LogWebAction(lowellReference, userId, WebActionType.ViewLFLLetter);
        }

        public async Task LogSaveLFLLetter(string lowellReference, string userId)
        {
            await LogWebAction(lowellReference, userId, WebActionType.SaveLFLLetter);
        }

        public async Task LogOpenBankingAccessed(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.OpenBankingAccessed);
        }

        public async Task LogOpenBankingComplete(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.OpenBankingComplete);
        }

        public async Task LogOpenBankingError(string lowellRef, string userId)
        {
            await LogWebAction(lowellRef, userId, WebActionType.OpenBankingError);
        }
    }
}