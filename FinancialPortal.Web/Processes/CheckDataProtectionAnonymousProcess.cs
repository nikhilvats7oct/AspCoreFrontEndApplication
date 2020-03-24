using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;

namespace FinancialPortal.Web.Processes
{
    public class CheckDataProtectionAnonymousProcess : ICheckDataProtectionAnonymousProcess
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;

        public CheckDataProtectionAnonymousProcess(IApiGatewayProxy apiGatewayProxy)
        {
            _apiGatewayProxy = apiGatewayProxy;
        }

        public async Task<ResultDto> CheckDataProtection(DataProtectionDto dataProtectionDto)
        {
            return await _apiGatewayProxy.CheckDataProtectionAnonymous(dataProtectionDto);
        }
    }
}
