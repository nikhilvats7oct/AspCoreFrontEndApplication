using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Services
{
    // Abstracts session state so that it can be plugged into service layer
    public class ApplicationSessionState : IApplicationSessionState
    {
        private readonly ISession _session;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationSessionState(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public string UserId
        {
            get
            {
                var caseflowUerIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("caseflow_userid");
                var userId = caseflowUerIdClaim?.Value;
                return userId;
            }
        }

        public void SaveHasPassedDataProtection()
        {
            _session.SetString(SessionKey.HasPassedDataProtection, "true");
        }

        public Guid AddLowellReferenceSurrogateKey(string lowellReference)
        {
            var lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);

            // mapper class will handle null session state
            var mapper = new LowellReferenceSurrogateKeyMap(lowellReferenceObject);
            var surrogateKey = mapper.AddLowellReferenceSurrogateKey(lowellReference);

            _session.SetString(SessionKey.LowellReferenceSurrogate, mapper.SerialiseAsJson());

            return surrogateKey;
        }

        public Guid? GetSurrogateKeyFromLowellReference(string lowellReference)
        {
            var lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);
            var mapper = new LowellReferenceSurrogateKeyMap(lowellReferenceObject);

            return mapper.GetSurrogateKeyFromLowellReference(lowellReference);
        }

        // Returns mappings (allowing reverse lookup - surrogate key by lowell ref)
        public IDictionary<string, Guid> AddLowellReferenceSurrogateKeys(IEnumerable<string> lowellReferences)
        {
            object lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);

            // mapper class will handle null session state
            var mapper = new LowellReferenceSurrogateKeyMap((string)lowellReferenceObject);
            var reverseMappings = mapper.AddLowellReferenceSurrogateKeys(lowellReferences);

            _session.SetString(SessionKey.LowellReferenceSurrogate, mapper.SerialiseAsJson());

            return reverseMappings;
        }

        public string GetLowellReferenceFromSurrogate(Guid surrogateKey)
        {
            object lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);

            // mapper class will handle null session state
            var mapper = new LowellReferenceSurrogateKeyMap((string)lowellReferenceObject);
            return mapper.GetLowellReferenceFromSurrogate(surrogateKey);
        }

        public string GetTopLowellReference()
        {
            object lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);

            // mapper class will handle null session state
            var mapper = new LowellReferenceSurrogateKeyMap((string)lowellReferenceObject);
            return mapper.GetTopLowellReference();
        }

        public Guid? GetTopLowellSurrogateKey()
        {
            object lowellReferenceObject = _session.GetString(SessionKey.LowellReferenceSurrogate);

            // mapper class will handle null session state
            var mapper = new LowellReferenceSurrogateKeyMap((string)lowellReferenceObject);
            return mapper.GetTopLowellSurrogateKey();
        }

        public void CheckSessionStatus(string surrogateKey)
        {
            //handles session expiry.
            GetLowellReferenceFromSurrogate(new Guid(surrogateKey));
        }

        public string SessionId
        {
            get
            {
                return this._session.Id;
            }
        }

        public string GetLoggedInLowellRef()
        {
            try
            {
                return _session.GetString(SessionKey.LoggedInLowellRef);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public List<Transaction> GetTransactions(string lowellReference)
        {
            var transactions = _session.GetString(SessionKey.Transactions(lowellReference));

            if (string.IsNullOrWhiteSpace(transactions)) { return null; }

            return JsonConvert.DeserializeObject<List<Transaction>>(transactions);
        }

        public void SaveTransactions(List<Transaction> transactions, string lowellRef)
        {
            string key = SessionKey.Transactions(lowellRef);
            _session.SetString(key, JsonConvert.SerializeObject(transactions));
        }

        public void SaveExpenditureMetrics(ExpenditureMetrics guideLines)
        {
            _session.SetString(SessionKey.ExpenditureMetrics, JsonConvert.SerializeObject(guideLines));
        }

        public Account GetAccount(string lowellReference)
        {
            var account = _session.GetString(SessionKey.Account(lowellReference));

            return string.IsNullOrWhiteSpace(account)
                ? null
                : JsonConvert.DeserializeObject<Account>(account);
        }

        public void SaveAccount(Account account, string lowellReference)
        {
            string key = SessionKey.Account(lowellReference);
            _session.SetString(key, JsonConvert.SerializeObject(account));
        }

        public ExpenditureMetrics GetExpenditureMetrics()
        {
            var triggers = _session.GetString(SessionKey.ExpenditureMetrics);

            return string.IsNullOrWhiteSpace(triggers)
                ? null
                : JsonConvert.DeserializeObject<ExpenditureMetrics>(triggers);
        }

        public void SaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, Guid surrogateLowellReference)
        {
            string key = $"{SessionKey.IncomeAndExpenditure}_{surrogateLowellReference}";
            if (key != null)
            {
                _session.SetString(key, JsonConvert.SerializeObject(incomeAndExpenditure));
            }
        }

        public IncomeAndExpenditure GetIncomeAndExpenditure(Guid surrogateLowellReference)
        {
            string key = $"{SessionKey.IncomeAndExpenditure}_{surrogateLowellReference}";
            if (key == null)
            {
                return null;
            }

            string json = _session.GetString(key);

            return String.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject<IncomeAndExpenditure>(json);
        }

        public bool LogPaymentResult
        {
            get => _session.GetString(SessionKey.LogPaymentResult) != null &&
                   bool.Parse(_session.GetString(SessionKey.LogPaymentResult));

            set => _session.SetString(SessionKey.LogPaymentResult, value.ToString());
        }

        public bool IandELaunchedExternally
        {
            get => _session.GetString(SessionKey.IandELaunchedExternally) != null &&
                bool.Parse(_session.GetString(SessionKey.IandELaunchedExternally));

            set => _session.SetString(SessionKey.IandELaunchedExternally, value.ToString());
        }

        public bool LogSetUpPlanResult
        {
            get => _session.GetString(SessionKey.LogSetUpPlanResult) != null &&
                   bool.Parse(_session.GetString(SessionKey.LogSetUpPlanResult));

            set => _session.SetString(SessionKey.LogSetUpPlanResult, value.ToString());
        }

        public List<AccountSummary> GetAccountSummaries()
        {
            var account = _session.GetString(SessionKey.Accounts(UserId));

            return string.IsNullOrWhiteSpace(account) ? null : JsonConvert.DeserializeObject<List<AccountSummary>>(account);
        }

        public void SaveAccountSummaries(List<AccountSummary> accounts)
        {
            string key = SessionKey.Account(UserId);
            _session.SetString(key, JsonConvert.SerializeObject(accounts));
        }

        public List<DocumentItem> GetDocumentsForAccount(string lowellReference)
        {
            var account = _session.GetString(SessionKey.Documents(lowellReference));

            return string.IsNullOrWhiteSpace(account) ? null : JsonConvert.DeserializeObject<List<DocumentItem>>(account);
        }

        public void SaveDocumentsForAccount(List<DocumentItem> model, string lowellReference)
        {
            string key = SessionKey.Documents(lowellReference);
            _session.SetString(key, JsonConvert.SerializeObject(model));
        }

        public void RemoveDocumentsForAccount(string lowellReference)
        {
            string key = SessionKey.Documents(lowellReference);
            _session.Remove(key);
        }

        // Separate class to make names tidy
        private class SessionKey
        {
            internal const string LoggedInLowellRef = "llr-77789cf3-91a1-44e1-952e-48a986ecbb8a";
            internal const string LowellReferenceSurrogate = "lrs-a61289e1-4e48-47e2-add6-c5b21b765778";
            internal const string HasPassedDataProtection = "pdp-24da84f2-8c5a-4fd1-b6ef-f229638fe43b";
            internal const string AccountAlreadyWebRegisteredMessage = "aawrm-24da84f2-8c5a-4fd1-b6ef-f229638fe43b";
            internal const string ExpenditureMetrics = "guidelines-11333137-c718-4708-9683-55e5da121bc2";
            internal static readonly string BillsAndOutgoings = "billsAndOutgoings-64df67d2-55f1-4678-be9d-6b247a384284_";
            internal static readonly string IncomeAndExpenditure = "incomeAndExpenditure-D21EFE1F-DBD3-4A18-AD1A-845F2EA29ABF";
            internal static readonly string HouseholdDetails = "household-9e267ddf-a121-4c32-b859-304ab1e23e8b";
            internal static readonly string Income = "income-a5fd05f3-775c-4b10-8471-0b72886672b8";
            internal static readonly string Expenditure = "expenditure-eccc6677-83d0-4ef0-85c1-c5dbd89143f1_";
            internal static readonly string LogPaymentResult = "lpr-D2ADB237-5239-4A08-A46E-33659E1D2162";
            internal static readonly string LogSetUpPlanResult = "spr-88D4CAEB-2B2F-42A8-9F84-7AA1D54501D1";
            internal static readonly string IandELaunchedExternally = "i&eExt-B9E3BD4F-8293-4A7F-997D-126C2631CA61";

            internal static string Transactions(string lowellReference) => "tra-597135fa-6cc3-41e2-a83d-e1dd55f6611d_" + lowellReference;
            internal static string Account(string lowellReference) => "account-597135fa-6cc3-41e2-a83d-e1dd55f6611d_" + lowellReference;
            internal static string Accounts(string userId) => "accounts-d90addba-a6b5-4aa3-aeee-eebb48ac379f_" + userId;
            internal static string Documents(string lowellReference) => "documents-c89ed500-b6e5-4585-9a8d-e84055e71452_" + lowellReference;
        }
    }
}