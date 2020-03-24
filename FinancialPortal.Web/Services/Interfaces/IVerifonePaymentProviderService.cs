using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IVerifonePaymentProviderService : IDisposable
    {
        string CreatePayload(PaymentOptionsVm accountVm);

        Task AddVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto);

        Task<VerifoneTransactionDto> GetVerifoneTransactionAsync(string transactionGuid);

        Task UpdateVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto);
    }
}
