using System;
using System.Threading.Tasks;
using FinancialPortal.Web.DataTransferObjects;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.ApiModels;

namespace FinancialPortal.Web.Proxy.Interfaces
{
    public interface IApiGatewayProxy
    {
        Task<DateTime> HitHeartBeat();
        Task<HeartBeatDto> CheckAllServices();
        Task CompleteRegistration(CompleteRegistrationDto dto);
        Task<ResultDto> CheckDataProtectionAnonymous(DataProtectionDto dto);
        Task<PaymentOptionsDto> GetPaymentOptions(AccountReferenceDto dto);
        Task<AmendDirectDebitPaymentDto> GetCurrentDirectDebit(AccountReferenceDto dto);
        Task<GetBankAccountValidationResultDto> CheckBankAccount(BankAccountCheckerDto dto);
        Task MakePayment(PaymentDto paymentDto);

        Task<ResultDto> SendContactUsMessage(ContactUsDetailsDto dto);
        Task<ResultDto> SendCallbackMessage(CallbackDetailsDto dto);

        Task SetupDirectDebitPlanAsync(DirectDebitPaymentDto dto);
        Task AmendDirectDebitPlanAsync(DirectDebitPaymentDto dto);
        Task<UserDto> GetUser(string userId);
        Task<VerifoneTransactionDto> GetVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto);
        Task AddVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto);
        Task UpdateVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto);

        Task<IncomeAndExpenditureApiModel> GetIncomeAndExpenditure(string lowellReference);
        Task SendToRabbitMQ(WebActionDto dto);
    }
}
