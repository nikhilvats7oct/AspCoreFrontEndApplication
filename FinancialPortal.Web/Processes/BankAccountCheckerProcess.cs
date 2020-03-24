using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class BankAccountCheckerProcess : IBankAccountCheckerProcess
    {
        private readonly ILogger<BankAccountCheckerProcess> _logger;
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public BankAccountCheckerProcess(ILogger<BankAccountCheckerProcess> logger,
                                         IApiGatewayProxy apiGatewayProxy)
        {
            _logger = logger;
            _apiGatewayProxy = apiGatewayProxy;
        }


        public async Task<GetBankAccountValidationResultDto> CheckBankAccount(BankAccountCheckerDto bankAccountCheckerDto)
        {
            return await _apiGatewayProxy.CheckBankAccount(bankAccountCheckerDto);
        }
    }
}
