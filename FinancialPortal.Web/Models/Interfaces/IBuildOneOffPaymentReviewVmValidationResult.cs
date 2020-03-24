using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Models.Interfaces
{
    public interface IBuildOneOffPaymentReviewVmValidationResult
    {
        bool IsValid { get; }
        OneOffPaymentReviewVm OneOffPaymentReviewVm { get; }
    }
}
