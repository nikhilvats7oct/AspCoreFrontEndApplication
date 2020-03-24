using AutoMapper;
using FinancialPortal.Models.ViewModels;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using X.PagedList;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class MyDocumentsController : BaseController
    {
        private readonly IDocumentsService _documentsService;
        private readonly IAccountsService _accountsService;
        private readonly IWebActivityService _webActivityService;
        private readonly IMapper _mapper;
        private readonly PortalSetting _setting;

        public MyDocumentsController(ILogger<BaseController> logger,
                                     IDistributedCache distributedCache,
                                     IApplicationSessionState sessionState,
                                     IConfiguration configuration,
                                     IDocumentsService documentsService,
                                     IAccountsService accountsService,
                                     IWebActivityService webActivityService,
                                     IMapper mapper,
                                     PortalSetting setting)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _documentsService = documentsService;
            _accountsService = accountsService;
            _webActivityService = webActivityService;
            _mapper = mapper;
            _setting = setting;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get all accounts for this user and Cache it.
            var allAccounts = await GetAccounts();

            // Find an account in random to assume to be selected.
            var selectedAccount = allAccounts.FirstOrDefault();
            var viewModel = _mapper.Map<MyDocumentsVm>(selectedAccount);

            // Get all available documents for Selected Account
            var documents = await GetDocuments(selectedAccount.Reference);

            // Map from List<DocumentItem> to MyDocumentsVm
            var pagedList = _mapper.Map<List<DocumentVm>>(documents)?.ToPagedList(1, 10);
            viewModel = _mapper.Map(pagedList, viewModel);

            // Map linked accounts
            var linkedAccounts = MapLinkedAccounts(allAccounts, selectedAccount.Reference);
            viewModel = _mapper.Map(linkedAccounts, viewModel);

            viewModel.AccountsHaveUnreadDocuments = allAccounts.Any(x => x.UnreadDocuments);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeAccount(string accountReference)
        {
            // Get all accounts for this user and Cache it.
            var allAccounts = await GetAccounts();

            // Get lowell reference
            var lowellReference = GetLowellReferenceFromSessionState(accountReference);

            // Find an account in random to assume to be selected.
            var selectedAccount = allAccounts.Where(x => x.Reference == lowellReference).FirstOrDefault();
            var viewModel = _mapper.Map<MyDocumentsVm>(selectedAccount);

            // Get all available documents for Selected Account
            var documents = await GetDocuments(lowellReference);
            var pagedList = _mapper.Map<List<DocumentVm>>(documents)?.ToPagedList(1, 10);
            viewModel = _mapper.Map(pagedList, viewModel);

            // Map linked accounts
            var linkedAccounts = MapLinkedAccounts(allAccounts, lowellReference);
            viewModel = _mapper.Map(linkedAccounts, viewModel);

            viewModel.AccountsHaveUnreadDocuments = allAccounts.Any(x => x.UnreadDocuments);

            return View("Index", viewModel);
        }

        public async Task<IActionResult> FilterItems(FilterDocumentsVm filterItems,
                                                    string accountReference,
                                                    string orderByReceived,
                                                    string orderByRead,
                                                    string orderBySubject,
                                                    DateTime? dateFrom,
                                                    DateTime? dateTo,
                                                    int pageNumber = 1)
        {

            // 1. Get all accounts for this user and Cache it.
            var allAccounts = await GetAccounts();

            // 2. Get lowell reference
            var lowellReference = GetLowellReferenceFromSessionState(accountReference);

            // 3. Find an account in random to assume to be selected.
            var selectedAccount = allAccounts.Where(x => x.Reference == lowellReference).FirstOrDefault();
            var viewModel = _mapper.Map<MyDocumentsVm>(selectedAccount);

            // 4. Get all available documents for Selected Account
            var documents = await GetDocuments(lowellReference);
            var pagedList = _mapper.Map<List<DocumentVm>>(documents)?.ToPagedList(pageNumber, 10);
            viewModel = _mapper.Map(pagedList, viewModel);

            //5. Map linked accounts
            var linkedAccounts = MapLinkedAccounts(allAccounts, lowellReference);
            viewModel = _mapper.Map(linkedAccounts, viewModel);
            viewModel = _mapper.Map(filterItems, viewModel);
            viewModel.AccountsHaveUnreadDocuments = allAccounts.Any(x => x.UnreadDocuments);

            //5. Check model state
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            //6. Map order by vm to filter items
            var mappedFilter = _mapper.Map<FilterDocumentsVm>(new OrderByVm
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                OrderByReceived = orderByReceived,
                OrderByRead = orderByRead,
                OrderBySubject = orderBySubject
            });

            //6. Merge both order by vm and filter items
            filterItems = _mapper.Map(mappedFilter, filterItems);

            //7. Filter documents
            documents = FilterOrderDocuments(filterItems, documents);

            pagedList = _mapper.Map<List<DocumentVm>>(documents)?.ToPagedList(pageNumber, 10);
            viewModel = _mapper.Map(pagedList, viewModel);
            viewModel = _mapper.Map(filterItems, viewModel);

            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDocuments([FromQuery] OrderByVm orderByVm)
        {
            // Get all accounts for this user and Cache it.
            var allAccounts = await GetAccounts();

            // Find an account in random to assume to be selected.
            var lowellReference = GetLowellReferenceFromSessionState(orderByVm.AccountReference);
            var selectedAccount = allAccounts.Where(x => x.Reference == lowellReference).FirstOrDefault();
            var viewModel = _mapper.Map<MyDocumentsVm>(selectedAccount);

            // Get all available documents for Selected Account
            var documents = await GetDocuments(lowellReference);

            // Map linked accounts
            var linkedAccounts = MapLinkedAccounts(allAccounts, lowellReference);
            viewModel = _mapper.Map(linkedAccounts, viewModel);

            // Map to filter items
            var filterItems = _mapper.Map<OrderByVm, FilterDocumentsVm>(orderByVm);

            // Order documents
            documents = FilterOrderDocuments(filterItems, documents);

            var pagedList = _mapper.Map<List<DocumentVm>>(documents)?.ToPagedList(1, 10);
            viewModel = _mapper.Map(pagedList, viewModel);
            viewModel = _mapper.Map(filterItems, viewModel);

            viewModel.AccountsHaveUnreadDocuments = allAccounts.Any(x => x.UnreadDocuments);

            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument(string accountReference, int documentId)
        {
            // Get lowell reference from cache
            var lowellReference = GetLowellReferenceFromSessionState(accountReference);

            // Get Pdf from caseflow
            var pdfAsStream = await _documentsService.DownloadDocument(lowellReference, documentId);

            // Converted pdf to bytes
            var pdf = SteamToBytes(pdfAsStream);

            // Log to weblogs
            await LogSaveLFLLetter(lowellReference, documentId);

            return File(new MemoryStream(pdf), "application/pdf", $"{lowellReference}_{DateTime.UtcNow:ddMMMyyyyHHmmss}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> OpenDocument(string accountReference, int documentId)
        {
            // Get lowell reference from cache
            var lowellReference = GetLowellReferenceFromSessionState(accountReference);

            // Get Pdf from caseflow
            var pdfAsStream = await _documentsService.DownloadDocument(lowellReference, documentId);

            // Converted pdf to bytes
            var pdf = SteamToBytes(pdfAsStream);

            // Update caseflow
            var document = await UpdateDocument(lowellReference, documentId);

            // Log to weblogs
            await LogViewLFLLetter(lowellReference, document);

            // Set response header
            SetResponseHeader(lowellReference);

            return File(pdf, contentType: MediaTypeNames.Application.Pdf);
        }

        private async Task<List<DocumentAccountsVm>> GetAccounts()
        {
            var accounts = ApplicationSessionState.GetAccountSummaries();

            if (accounts is null)
            {
                accounts = await _accountsService.GetAccounts(ApplicationSessionState.UserId);
                accounts = accounts.Where(x => !x.AccountStatusIsWithSolicitors).ToList();
                ApplicationSessionState.SaveAccountSummaries(accounts);
            }

            var allMappedAccounts = _mapper.Map<List<DocumentAccountsVm>>(accounts)
                                           .OrderBy(d => d.AccountName).ThenBy(x => x.AccountReference)
                                           .ToList();

            foreach (var account in allMappedAccounts)
            {
                account.AccountReference = ApplicationSessionState.GetSurrogateKeyFromLowellReference(account.AccountReference).ToString();
            }

            return allMappedAccounts;
        }

        private List<LinkedAccountsVm> MapLinkedAccounts(List<DocumentAccountsVm> allAccounts, string lowellReference)
        {
            List<LinkedAccountsVm> linkedAccounts = _mapper.Map<List<LinkedAccountsVm>>(allAccounts)
                                                           .OrderBy(x => x.Account.Key)
                                                           .ThenBy(x => x.Account.Value).ToList();

            var selectedAccount = linkedAccounts.Find(x => x.Reference == lowellReference);
            selectedAccount.IsSelected = true;

            return linkedAccounts;
        }

        private async Task<List<DocumentItem>> GetDocuments(string lowellReference)
        {
            var documentsForAccount = ApplicationSessionState.GetDocumentsForAccount(lowellReference);

            if (documentsForAccount is null)
            {
                documentsForAccount = await _documentsService.GetMyDocuments(lowellReference);

                if (documentsForAccount != null)
                {
                    ApplicationSessionState.SaveDocumentsForAccount(documentsForAccount, lowellReference);
                }
            }

            return documentsForAccount?.OrderByDescending(x => x.Received).ToList() ?? new List<DocumentItem>();
        }

        private async Task<DocumentItem> UpdateDocument(string lowellReference, int documentId)
        {
            var documents = await GetDocuments(lowellReference);

            var index = documents.FindIndex(x => x.Id == documentId);

            ApplicationSessionState.RemoveDocumentsForAccount(lowellReference);

            if (documents.ElementAt(index).Read == null)
            {
                await _documentsService.UpdateDocument(lowellReference, documentId);
            }

            return documents.ElementAt(index);
        }

        private string GetLowellReferenceFromSessionState(string surrogateKey)
        {
            var guid = new Guid(surrogateKey);
            var lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            return lowellReference;
        }

        private List<DocumentItem> FilterOrderDocuments(FilterDocumentsVm filterItems, List<DocumentItem> documents)
        {
            if (documents != null)
            {
                if (filterItems.DateFrom != null)
                {
                    documents = documents.Where(x => x.Received.Value.Date >= filterItems.DateFrom).ToList();
                }

                if (filterItems.DateTo != null)
                {
                    documents = documents.Where(x => x.Received.Value.Date <= filterItems.DateTo).ToList();
                }

                if (!string.IsNullOrEmpty(filterItems.KeyWord))
                {
                    documents = documents.Where(x => x.Subject.ToLower().Contains(filterItems.KeyWord.ToLower()))
                                         .ToList();
                }

                if (filterItems.OrderByReceived != null)
                {
                    switch (filterItems.OrderByReceived)
                    {
                        case "ascending": documents = documents.OrderBy(x => x.Received).ToList(); break;
                        case "descending": documents = documents.OrderByDescending(x => x.Received).ToList(); break;
                    }

                    filterItems.OrderByReceived = filterItems.OrderByReceived == "ascending" ? "descending" : "ascending";
                }

                if (filterItems.OrderByRead != null)
                {
                    switch (filterItems.OrderByRead)
                    {
                        case "ascending": documents = documents.OrderBy(x => x.Read).ToList(); break;
                        case "descending": documents = documents.OrderByDescending(x => x.Read).ToList(); break;
                    }

                    filterItems.OrderByRead = filterItems.OrderByRead == "ascending" ? "descending" : "ascending";
                }

                if (filterItems.OrderBySubject != null)
                {
                    switch (filterItems.OrderBySubject)
                    {
                        case "ascending": documents = documents.OrderBy(x => x.Subject).ToList(); break;
                        case "descending": documents = documents.OrderByDescending(x => x.Subject).ToList(); break;
                    }

                    filterItems.OrderBySubject = filterItems.OrderBySubject == "ascending" ? "descending" : "ascending";
                }

                if (!documents.Any())
                {
                    filterItems.Message = "No Results Found";
                }

                return documents;
            }

            return new List<DocumentItem>();
        }

        private byte[] SteamToBytes(Stream file)
        {
            byte[] result;
            using (var streamReader = new MemoryStream())
            {
                file.CopyTo(streamReader);
                result = streamReader.ToArray();
            }

            return result;
        }

        private void SetResponseHeader(string lowellReference)
        {
            var contentDispositionHeader = new ContentDisposition
            {
                Inline = true,
                FileName = $"{lowellReference}_{DateTime.UtcNow:ddMMMyyyyHHmmss}.pdf"
            };

            Response.Headers.Add("Content-Type", "application/pdf");
            Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());
        }

        private async Task LogSaveLFLLetter(string lowellReference, int documentId)
        {
            var documents = await GetDocuments(lowellReference);
            var isCustomer = documents.Where(x => x.Id == documentId).First().IsCustomer;
            if (isCustomer)
            {
                await _webActivityService.LogSaveLFLLetter(null, GetCaseflowUserId());
            }
            else
            {
                await _webActivityService.LogSaveLFLLetter(lowellReference, GetCaseflowUserId());
            }
        }

        private async Task LogViewLFLLetter(string lowellReference, DocumentItem document)
        {
            if (document.IsCustomer)
            {
                await _webActivityService.LogViewLFLLetter(null, GetCaseflowUserId());
            }
            else
            {
                await _webActivityService.LogViewLFLLetter(lowellReference, GetCaseflowUserId());
            }
        }
    }
}
