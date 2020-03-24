using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FinancialPortal.Web.DataTransferObjects;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Proxy
{
    [ExcludeFromCodeCoverage]
    public class ApiGatewayProxy : IApiGatewayProxy
    {
        private readonly string _baseUrl;
        private readonly IRestClient _restClient;

        public ApiGatewayProxy(
            ILogger<ApiGatewayProxy> logger,
            IRestClient restClient,
            string baseUrl)
        {
            _restClient = restClient;
            _baseUrl = baseUrl;
        }

        public async Task<DateTime> HitHeartBeat()
        {
            var innerUrl = $"{_baseUrl}api/heartbeat";
            return await _restClient.GetAsync<DateTime>(innerUrl);
        }

        public async Task<HeartBeatDto> CheckAllServices()
        {
            var innerUrl = $"{_baseUrl}api/heartbeat/checkallservices";
            return await _restClient.GetAsync<HeartBeatDto>(innerUrl);
        }

        public async Task CompleteRegistration(CompleteRegistrationDto dto)
        {
            var innerUrl = $"{_baseUrl}api/Register/CompleteRegistration";
            await _restClient.PostNoResponseAsync(innerUrl, dto);
        }

        public async Task<ResultDto> CheckDataProtectionAnonymous(DataProtectionDto dto)
        {
            var innerUrl = $"{_baseUrl}api/DataProtection/CheckDataProtectionAnonymous";
            return await _restClient.PostAsync<DataProtectionDto, ResultDto>(innerUrl, dto);
        }

        public async Task<PaymentOptionsDto> GetPaymentOptions(AccountReferenceDto dto)
        {
            var innerUrl = $"{_baseUrl}api/PaymentOptions/GetPaymentOptions";
            return await _restClient.PostAsync<AccountReferenceDto, PaymentOptionsDto>(innerUrl, dto);
        }

        public async Task<AmendDirectDebitPaymentDto> GetCurrentDirectDebit(AccountReferenceDto dto)
        {
            var innerUrl = $"{_baseUrl}api/AmendDirectDebit/GetCurrentDirectDebit";
            return await _restClient.PostAsync<AccountReferenceDto, AmendDirectDebitPaymentDto>(innerUrl, dto);
        }

        public async Task<GetBankAccountValidationResultDto> CheckBankAccount(
            BankAccountCheckerDto bankAccountCheckerDto)
        {
            var innerUrl = $"{_baseUrl}api/BankAccountChecker/CheckBankAccount/";
            return await _restClient.PostAsync<BankAccountCheckerDto, GetBankAccountValidationResultDto>(
                innerUrl, bankAccountCheckerDto);
        }

        public async Task MakePayment(PaymentDto paymentDto)
        {
            var innerUrl = $"{_baseUrl}api/Payment/MakePayment";

            await _restClient.PostNoResponseAsync(innerUrl, paymentDto);
        }

        public async Task<UserDto> GetUser(string userId)
        {
            var innerUrl = $"{_baseUrl}api/Users/GetUser";
            return await _restClient.PostAsync<string, UserDto>(innerUrl, userId);
        }

        public async Task SetupDirectDebitPlanAsync(DirectDebitPaymentDto dto)
        {
            var innerUrl = $"{_baseUrl}api/DirectDebit/SetupPlan";

            await _restClient.PostNoResponseAsync(innerUrl, dto);
        }

        public async Task AmendDirectDebitPlanAsync(DirectDebitPaymentDto dto)
        {
            var innerUrl = $"{_baseUrl}api/AmendDirectDebit/AmendPlan";

            await _restClient.PostNoResponseAsync(innerUrl, dto);
        }

        public async Task<VerifoneTransactionDto> GetVerifoneTransactionAsync(
            VerifoneTransactionDto verifoneTransactionDto)
        {
            var innerUrl = $"{_baseUrl}api/VerifonePayment/GetTransaction";

            return await _restClient.PostAsync<VerifoneTransactionDto, VerifoneTransactionDto>(innerUrl,
                verifoneTransactionDto);
        }

        public async Task AddVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto)
        {
            var innerUrl = $"{_baseUrl}api/VerifonePayment/AddTransaction";

            await _restClient.PostNoResponseAsync(innerUrl, verifoneTransactionDto);
        }

        public async Task UpdateVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto)
        {
            var innerUrl = $"{_baseUrl}api/VerifonePayment/UpdateTransaction";

            await _restClient.PostNoResponseAsync(innerUrl, verifoneTransactionDto);
        }

        public async Task SendToRabbitMQ(WebActionDto dto)
        {
            var innerUrl = $"{_baseUrl}api/WebActivity/SendToRabbitMQ";

            await _restClient.PostNoResponseAsync(innerUrl, new
            {
                dto.LowellReference,
                dto.Company,
                dto.DateTime,
                WebActionID = (int)dto.WebActionType,
                dto.Guid
            });
        }

        public async Task<IncomeAndExpenditureApiModel> GetIncomeAndExpenditure(string lowellReference)
        {
            var innerUrl = $"{_baseUrl}api/BudgetCalculator/GetSavedIncomeAndExpenditure";

            var dto = new IncomeAndExpenditureApiRequest
            {
                LowellReference = lowellReference
            };

            return await _restClient.PostAsync<IncomeAndExpenditureApiRequest, IncomeAndExpenditureApiModel>(innerUrl,
                dto);
        }

        public async Task<ResultDto> SendContactUsMessage(ContactUsDetailsDto dto)
        {
            var innerUrl = $"{_baseUrl}api/ContactUs";

            return await _restClient.PostAsync<ContactUsDetailsDto, ResultDto>(innerUrl, dto);
        }

        public async Task<ResultDto> SendCallbackMessage(CallbackDetailsDto dto)
        {
            var innerUrl = $"{_baseUrl}api/Callback";

            return await _restClient.PostAsync<CallbackDetailsDto, ResultDto>(innerUrl, dto);
        }
    }
}