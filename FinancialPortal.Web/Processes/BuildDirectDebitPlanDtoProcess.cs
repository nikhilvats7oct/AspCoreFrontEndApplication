using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class BuildDirectDebitPlanDtoProcess : IBuildDirectDebitPlanDtoProcess
    {
        private readonly ILogger<BuildDirectDebitPlanDtoProcess> _logger;

        public BuildDirectDebitPlanDtoProcess(ILogger<BuildDirectDebitPlanDtoProcess> logger)
        {
            _logger = logger;
        }

        public DirectDebitPaymentDto BuildDirectDebitPlanDto(DirectDebitPlanOverviewVm directDebitPlanOverviewVm)
        {
            var dto = new DirectDebitPaymentDto()
            {
                SortCode = directDebitPlanOverviewVm.SortCode,
                AccountNumber = directDebitPlanOverviewVm.AccountNumber,
                AccountHoldersName = directDebitPlanOverviewVm.AccountHoldersName,
                LowellReference = directDebitPlanOverviewVm.LowellReference,
                PaymentType = directDebitPlanOverviewVm.PaymentType,
                StartDate = directDebitPlanOverviewVm.StartDate,
                PaymentAmount = directDebitPlanOverviewVm.PaymentAmount,
                Frequency = directDebitPlanOverviewVm.Frequency,
                GuaranteeRead = directDebitPlanOverviewVm.GuaranteeRead,
                PlanTotal = directDebitPlanOverviewVm.PlanTotal,
                DiscountAccepted = directDebitPlanOverviewVm.DiscountAccepted,
                DiscountAmount = directDebitPlanOverviewVm.DiscountAmount,
                EmailAddress = directDebitPlanOverviewVm.EmailAddress,
                User = directDebitPlanOverviewVm.UserLoggedIn ? "WebUser" : "WebAnon"
            };
            return dto;
        }
    }
}
