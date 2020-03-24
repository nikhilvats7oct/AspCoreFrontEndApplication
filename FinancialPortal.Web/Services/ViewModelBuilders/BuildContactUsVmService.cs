using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildContactUsVmService : IBuildContactUsVmService
    {
        private readonly ILogger<BuildContactUsVmService> _logger;

        private readonly IGetDaysForDoBProcess _getDaysForDoB;
        private readonly IGetMonthsForDoBProcess _getMonthsForDoB;
        private readonly IGetYearsForDoBProcess _getYearsForDoB;
        private readonly ContactUsSetting _contactUsSetting;

        public BuildContactUsVmService(
            ILogger<BuildContactUsVmService> logger,
            IGetDaysForDoBProcess getDaysForDoB,
            IGetMonthsForDoBProcess getMonthsForDoB,
            IGetYearsForDoBProcess getYearsForDoB,
            ContactUsSetting contactUsSetting)
        {
            _contactUsSetting = contactUsSetting;
            _logger = logger;
            _getDaysForDoB = getDaysForDoB;
            _getMonthsForDoB = getMonthsForDoB;
            _getYearsForDoB = getYearsForDoB;
        }

        public ContactUsVm CreateNewContactUsVm()
        {
            var querySelections = GetTopics();

            var accountHolderStatuses = GetAccountHoldersStatus();

            var contactUsVm = new ContactUsVm
            {
                AccountHolderStatuses = accountHolderStatuses,
                QueryTopicsSelections = querySelections
            };

            PopulateDateComponents(contactUsVm);

            return contactUsVm;
        }

        public void PopulateDateComponents(ContactUsVm existingVm)
        {
            existingVm.Days = _getDaysForDoB.Build();
            existingVm.Months = _getMonthsForDoB.Build();
            existingVm.Years = _getYearsForDoB.Build(1850, DateTime.Now.Year);
        }

        public ContactUsVm ReinstateContactUsVm(ContactUsVm model)
        {
            var topics = GetTopics();

            var accountHolderStatuses = GetAccountHoldersStatus();

            model.AccountHolderStatuses = accountHolderStatuses.ToList();

            model.QueryTopicsSelections = topics.ToList();

            PopulateDateComponents(model);

            return model;
        }

        private IEnumerable<string> GetTopics()
        {
            return _contactUsSetting.QueryAreas;
        }

        private List<KeyValuePair<string, string>> GetAccountHoldersStatus()
        {
            var accountHolderStatuses = new List<KeyValuePair<string, string>>();
            accountHolderStatuses.Add(new KeyValuePair<string, string>("Account Holder", "I am the account holder"));
            accountHolderStatuses.Add(new KeyValuePair<string, string>("Authorised 3rd Party", "I am an authorised third party on the account"));
            accountHolderStatuses.Add(new KeyValuePair<string, string>("Unauthorised 3rd Party", "I am a third party but I haven't been authorised"));

            return accountHolderStatuses;
        }
    }
}
