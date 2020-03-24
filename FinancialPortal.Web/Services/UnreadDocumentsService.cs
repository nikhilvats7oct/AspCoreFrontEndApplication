using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class UnreadDocumentsService : IUnreadDocumentsService
    {
        private readonly IAccountsService _accountsService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IApplicationSessionState _sessionState;

        public UnreadDocumentsService(IAccountsService accountsService,
                                      IHttpContextAccessor httpContext,
                                      IApplicationSessionState sessionState)
        {
            _accountsService = accountsService;
            _httpContext = httpContext;
            _sessionState = sessionState;
        }

        public async Task<bool> HasUnreadDocuments()
        {
            var accountSummaries = _sessionState.GetAccountSummaries();
            var userId = _sessionState.UserId;
            if (accountSummaries == null)
            {
                accountSummaries = await _accountsService.GetAccounts(userId);
                _sessionState.SaveAccountSummaries(accountSummaries);
            }

            var hasUnreadDocuments = accountSummaries.Where(x => !x.AccountStatusIsClosed && !x.AccountStatusIsWithSolicitors)
                                                     .Any(x => x.UnreadDocuments);

            return hasUnreadDocuments;
        }
    }
}
