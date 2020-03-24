using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface ISendPaymentProcess
    {
        Task SendPayment(PaymentDto paymentDto);
    }
}
