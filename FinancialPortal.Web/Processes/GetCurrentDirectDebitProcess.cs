using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;

namespace FinancialPortal.Web.Processes
{
    public class GetCurrentDirectDebitProcess : IGetCurrentDirectDebitProcess
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public GetCurrentDirectDebitProcess(IApiGatewayProxy apiGatewayProxy)
        {
            _apiGatewayProxy = apiGatewayProxy;
        }

        public async Task<AmendDirectDebitPaymentDto> GetCurrentDirectDebit(AccountReferenceDto accountReferenceDto)
        {
            return await _apiGatewayProxy.GetCurrentDirectDebit(accountReferenceDto);
        }
    }
}