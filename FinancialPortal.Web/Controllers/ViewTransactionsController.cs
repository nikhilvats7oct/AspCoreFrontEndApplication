using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Framework.Pdf.Aspose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace FinancialPortal.Web.Controllers
{

    public class ViewTransactionsController : BaseController
    {
        private readonly PortalSetting  _portalSetting;
        private readonly IGtmService _gtmService;
        private int _pageSize;
        private readonly ITransactionsService _transactionsService;
        private readonly IWebActivityService _webActivityService;
        private readonly IPdfGenerator<PdfViewModel> _pdfGenerator;
        private readonly IMapper _mapper;
        private readonly IAccountsService _accountsService;

        public ViewTransactionsController(ILogger<BaseController> logger,
            IConfiguration configuration,
            ITransactionsService transactionsService,
            PortalSetting  portalSetting,
            IGtmService gtmService,
            IPdfGenerator<PdfViewModel> pdfGenerator,
            IDistributedCache distributedCache,
            IWebActivityService webActivityService,
            IMapper mapper,
            IApplicationSessionState sessionState,
            IAccountsService accountsService)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _transactionsService = transactionsService;
            _portalSetting = portalSetting;
            _pageSize = _portalSetting.PageSizeViewTransactions;
            _gtmService = gtmService;
            _webActivityService = webActivityService;
            _pdfGenerator = pdfGenerator;
            _mapper = mapper;
            _accountsService = accountsService;
        }

        public async Task<IActionResult> Transactions(
            TransactionsVm transactionsVm, 
            int pageNumber = 1)
        {
            var value = RouteData.Values["id"];
            
            if (value == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid.TryParse(value.ToString(), out var id);

            if (!ModelState.IsValid)
            {
                return View(transactionsVm);
            }

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(id);
            if (string.IsNullOrEmpty(lowellReference))
            {
                throw new Exception("No lowell reference found in cache");
            }

            Account account = ApplicationSessionState.GetAccount(lowellReference);
            if (account == null)
            {
                account = await _accountsService.GetAccount(LoggedInUserId, lowellReference);
                ApplicationSessionState.SaveAccount(account, lowellReference);
            }

            List<Transaction> transactions = ApplicationSessionState.GetTransactions(lowellReference);
            if (transactions == null)
            {
                transactions = await _transactionsService.GetTransactions(account.AccountReference);
                ApplicationSessionState.SaveTransactions(transactions, lowellReference);
            }

            if (transactions == null) { transactions = new List<Transaction>(); }

            transactionsVm.AccountName = account.OriginalCompany;
            transactionsVm.AccountBalance = account.OutstandingBalance;
            transactionsVm.AccountReference = account.AccountReference;

            _gtmService.RaiseTransactionsViewedEvent(transactionsVm, LoggedInUserId, "Regular Account");
            await _webActivityService.LogAllTransactionsViewed(transactionsVm.AccountReference, LoggedInUserId);
            
            if (transactionsVm.FilterTransactions.DateFrom != null)
            {
                transactions = transactions.Where(x => x.Date >= transactionsVm.FilterTransactions.DateFrom).ToList();
            }

            if (transactionsVm.FilterTransactions.DateTo != null)
            {
                transactions = transactions.Where(x => x.Date <= transactionsVm.FilterTransactions.DateTo).ToList();
            }

            if (!string.IsNullOrEmpty(transactionsVm.FilterTransactions.KeyWord))
            {
                transactions = transactions.Where(x => x.Description.ToLower().Contains(transactionsVm.FilterTransactions.KeyWord.ToLower())).ToList();
            }

            transactions = transactions.OrderByDescending(x => x.Date).ToList();

            if (!transactions.Any())
            {
                transactionsVm.FilterTransactions.DateMessage = "No Results Found";
            }

            transactionsVm.PagedList = 
                _mapper.Map<List<Transaction>, List<TransactionVm>>(transactions).ToPagedList(pageNumber, _pageSize);

            transactionsVm.LoggedInUserID = LoggedInUserId;
            transactionsVm.LoggedInLowellRef = ApplicationSessionState.GetLoggedInLowellRef();

            return View(transactionsVm);
        }

        public async Task<IActionResult> Download(string accountRef)
        {
            var value = RouteData.Values["id"];

            if (value == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid.TryParse(value.ToString(), out var id);

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(id);
            if (string.IsNullOrEmpty(lowellReference))
            {
                throw new Exception("No lowell reference found in cache");
            }

            Account account = ApplicationSessionState.GetAccount(lowellReference);
            if (account == null)
            {
                account = await _accountsService.GetAccount(LoggedInUserId, lowellReference);
                ApplicationSessionState.SaveAccount(account, lowellReference);
            }

            List<Transaction> transactions = ApplicationSessionState.GetTransactions(lowellReference);                
            if (transactions == null)
            {
                transactions = await _transactionsService.GetTransactions(account.AccountReference);
                ApplicationSessionState.SaveTransactions(transactions, lowellReference);
            }        

            transactions.OrderByDescending(x => x.Date).ToList();

            var transactionsVm = _mapper.Map<List<Transaction>, List<TransactionVm>>(transactions);
            
            var vm = new PdfViewModel()
            {
                AccountBalance = account.OutstandingBalance,
                AccountName = account.OriginalCompany,
                AccountReference = account.AccountReference,
                PaymentDetails = transactionsVm
            };

            var pdf = _pdfGenerator.Generate(vm);

            await _webActivityService.LogStatementDownloaded(account.AccountReference, LoggedInUserId);

            return File(new MemoryStream(pdf), "application/pdf",
                $"{account.AccountReference}_{DateTime.UtcNow:ddMMMyyyyHHmmss}.pdf");
        }
    }
}