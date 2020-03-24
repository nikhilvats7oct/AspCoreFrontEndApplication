using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class DirectDebitPlanSetupService : IDirectDebitPlanSetupService
    {
        private readonly ILogger<DirectDebitPlanSetupService> _logger;
        private readonly IBuildDirectDebitPlanDtoProcess _directDebitPlanDtoProcess;
        private readonly ISendDirectDebitPlanProcess _sendDirectDebitPlanProcess;
        private readonly IBankAccountCheckerProcess _bankAccountCheckerProcess;
        private readonly ResultDto _resultDto;

        public DirectDebitPlanSetupService(ILogger<DirectDebitPlanSetupService> logger,
                                            IBuildDirectDebitPlanDtoProcess directDebitPlanDtoProcess,
                                            ISendDirectDebitPlanProcess sendDirectDebitPlanProcess,
                                            IBankAccountCheckerProcess bankAccountCheckerProcess)
        {
            _logger = logger;
            _directDebitPlanDtoProcess = directDebitPlanDtoProcess;
            _sendDirectDebitPlanProcess = sendDirectDebitPlanProcess;
            _bankAccountCheckerProcess = bankAccountCheckerProcess;
            _resultDto = new ResultDto();
        }

        public async Task CreateDirectDebitPlan(DirectDebitPlanOverviewVm directDebitPlanOverviewVm)
        {
            var directDebitPaymentDto = _directDebitPlanDtoProcess.BuildDirectDebitPlanDto(directDebitPlanOverviewVm);
            await _sendDirectDebitPlanProcess.SendDirectDebitPlanAsync(directDebitPaymentDto);
        }

        public async Task<ResultDto> CheckDirectDebitDetails(DirectDebitDetailsVm directDebitDetailsVm)
        {
            var bankAccountCheckerDetails = new BankAccountCheckerDto()
            {
                AccountNumber = directDebitDetailsVm.AccountNumber,
                SortCode = directDebitDetailsVm.SortCode
            };
            var backAccountDetailsConfirmed = await _bankAccountCheckerProcess.CheckBankAccount(bankAccountCheckerDetails);

            _resultDto.IsSuccessful = false;
            if (backAccountDetailsConfirmed.ValidationResult == BankAccountValidationResult.AccountInvalid)
            {
                _resultDto.MessageForUser = BankAccountCheckerConstants.AccountInvalid;
                return _resultDto;
            }
            if (backAccountDetailsConfirmed.ValidationResult == BankAccountValidationResult.ModulusCheckFailed)
            {
                _resultDto.MessageForUser = BankAccountCheckerConstants.ModulusCheckFailed;
                return _resultDto;
            }
            if (backAccountDetailsConfirmed.ValidationResult == BankAccountValidationResult.SortCodeInvalid)
            {
                _resultDto.MessageForUser = BankAccountCheckerConstants.SortCodeInvalid;
                return _resultDto;
            }

            _resultDto.IsSuccessful = true;
            return _resultDto;
        }
    }
}
