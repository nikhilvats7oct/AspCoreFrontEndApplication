using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Services.Interfaces.Utility
{
    public interface IApplicationSessionState
    {
        string UserId { get; }
        bool LogPaymentResult { get; set; }
        bool LogSetUpPlanResult { get; set; }
        string SessionId { get; }
        bool IandELaunchedExternally { get; set; }
        //
        // Lowell Reference Surrogate Keys
        //
        // When the user hits the web page showing all accounts, a set of anonymous GUIDs
        // are created with a mapping table to Lowell Reference stored in session state.
        // This enables the GUIDs to be included in URLS (as ID parameter).
        //
        // When anonymous, a single mapping is added so that account level pages
        // (e.g. Payment Options) will work consistently.
        //

        // Add methods return existing Guid(s) if Lowell References already present
        IDictionary<string, Guid> AddLowellReferenceSurrogateKeys(IEnumerable<string> lowellReferences);
        Guid AddLowellReferenceSurrogateKey(string lowellReference);
        string GetLowellReferenceFromSurrogate(Guid surrogateKey);
        void CheckSessionStatus(string surrogateKey);
        string GetLoggedInLowellRef();
        List<Transaction> GetTransactions(string lowellReference);
        void SaveTransactions(List<Transaction> transactions, string lowellRef);
        Account GetAccount(string lowellReference);
        void SaveAccount(Account account, string lowellReference);
        ExpenditureMetrics GetExpenditureMetrics();
        void SaveExpenditureMetrics(ExpenditureMetrics guideLines);
        void SaveHasPassedDataProtection();
        IncomeAndExpenditure GetIncomeAndExpenditure(Guid surrogateLowellReference);
        void SaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, Guid surrogateLowellReference);
        Guid? GetSurrogateKeyFromLowellReference(string lowellReference);
        string GetTopLowellReference();
        Guid? GetTopLowellSurrogateKey();
        List<AccountSummary> GetAccountSummaries();
        void SaveAccountSummaries(List<AccountSummary> accounts);
        List<DocumentItem> GetDocumentsForAccount(string lowellReference);
        void SaveDocumentsForAccount(List<DocumentItem> model, string lowellReference);
        void RemoveDocumentsForAccount(string lowellReference);
    }
}