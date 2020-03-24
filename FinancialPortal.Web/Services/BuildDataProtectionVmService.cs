using System;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class BuildDataProtectionVmService : IBuildDataProtectionVmService
    {
        private readonly ILogger<BuildDataProtectionVmService> _logger;
        private readonly IGetDaysForDoBProcess _getDaysForDoB;
        private readonly IGetMonthsForDoBProcess _getMonthsForDoB;
        private readonly IGetYearsForDoBProcess _getYearsForDoB;

        public BuildDataProtectionVmService(ILogger<BuildDataProtectionVmService> logger,
                                            IGetDaysForDoBProcess getDaysForDoB,
                                            IGetMonthsForDoBProcess getMonthsForDoB,
                                            IGetYearsForDoBProcess getYearsForDoB)
        {
            _logger = logger;
            _getDaysForDoB = getDaysForDoB;
            _getMonthsForDoB = getMonthsForDoB;
            _getYearsForDoB = getYearsForDoB;
        }

        public DataProtectionVm Build()
        {
            var model = new DataProtectionVm();
            PopulateDateComponents(model);

            return model;
        }

        public void PopulateDateComponents(DataProtectionVm existingVm)
        {
            existingVm.Days = _getDaysForDoB.Build();
            existingVm.Months = _getMonthsForDoB.Build();
            existingVm.Years = _getYearsForDoB.Build(1850, DateTime.Now.Year);
        }
    }
}