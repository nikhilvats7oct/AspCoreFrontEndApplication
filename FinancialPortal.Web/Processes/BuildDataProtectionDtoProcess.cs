using System.Diagnostics;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class BuildDataProtectionDtoProcess : IBuildDataProtectionDtoProcess
    {
        private readonly ILogger<BuildDataProtectionDtoProcess> _logger;

        public BuildDataProtectionDtoProcess(ILogger<BuildDataProtectionDtoProcess> logger)
        {
            _logger = logger;
        }

        public DataProtectionDto Build(DataProtectionVm model)
        {
            // Before we reach this point, the incoming VM should have been validated
            Debug.Assert(model.DateOfBirth.HasValue);

            var dto = new DataProtectionDto
            {
                LowellReference = model.LowellReference,
                Postcode = model.Postcode.ToUpper(),
                DateOfBirth = model.DateOfBirth.Value
            };

            return dto;
        }
    }
}