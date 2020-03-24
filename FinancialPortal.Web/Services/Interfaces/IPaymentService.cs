using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task MakePayment(PaymentResultVm model, OneOffPaymentDto oneOffPaymentReviewVm);
    }
}
