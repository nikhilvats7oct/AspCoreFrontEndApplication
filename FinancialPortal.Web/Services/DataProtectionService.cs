using System.Diagnostics;
using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services
{
    public class DataProtectionService : IDataProtectionService
    {
        private readonly ICheckDataProtectionAnonymousProcess _checkDataProtectionAnonymousProcess;

        public DataProtectionService(ICheckDataProtectionAnonymousProcess checkDataProtectionAnonymousProcess)
        {
            _checkDataProtectionAnonymousProcess = checkDataProtectionAnonymousProcess;
        }

        public async Task<ResultDto> CheckDataProtection(DataProtectionVm dataProtectionVm)
        {
            // VM will have been validated before we hit the service
            Debug.Assert(dataProtectionVm.DateOfBirth != null, "dataProtectionVm.DateOfBirth != null");

            DataProtectionDto dto = new DataProtectionDto()
            {
                LowellReference = dataProtectionVm.LowellReference,
                DateOfBirth = dataProtectionVm.DateOfBirth.Value,
                Postcode = dataProtectionVm.Postcode
            };

            return await _checkDataProtectionAnonymousProcess.CheckDataProtection(dto);
        }
    }
}
