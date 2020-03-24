using System;
using System.Collections.Generic;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildCallbackVmService : IBuildCallbackVmService
    {
        private readonly ILogger<BuildCallbackVmService> _logger;
        private readonly IGetTimeSlotsProcess _getTimeSlotsProcess;
        private readonly CallbackSetting _callbackSetting;

        public BuildCallbackVmService(
            ILogger<BuildCallbackVmService> logger,
            IGetTimeSlotsProcess getTimeSlotsProcess,
            CallbackSetting callbackSetting)
        {
            _logger = logger;
            _getTimeSlotsProcess = getTimeSlotsProcess;
            _callbackSetting = callbackSetting;
        }

        public CallbackVm CreateNewCallbackVm()
        {
            var accountHolderStatuses = GetAccountHoldersStatus();
            var callbackTimeStatuses = GetCallbackTimeStatus();

            var callbackVm = new CallbackVm
            {
                AccountHolderStatuses = accountHolderStatuses,
                CallbackOptions = callbackTimeStatuses
            };

            PopulateTimeComponents(callbackVm);

            return callbackVm;
        }

        private void PopulateTimeComponents(CallbackVm existingVm)
        {
            DateTime currDate = DateTime.UtcNow.ToLocalTime();
            existingVm.TimeSlotsSunday.Add(new KeyValuePair<string, string>(null, "Time slot"));

            if (currDate.DayOfWeek == DayOfWeek.Saturday)
            {
                existingVm.TimeSlotsToday = GetTimeSlots(
                    ToDateTime(_callbackSetting.TimeSlotSaturdayStartTime),
                    ToDateTime(_callbackSetting.TimeSlotSaturdayEndTime),
                    currDate,
                     2);
            }
            else if (currDate.DayOfWeek != DayOfWeek.Sunday)
            {
                existingVm.TimeSlotsToday = GetTimeSlots(
                    ToDateTime(_callbackSetting.TimeSlotWeekdayStartTime),
                    ToDateTime(_callbackSetting.TimeSlotWeekdayEndTime),
                    currDate,
                    2);
            }

            existingVm.FirstAvailableDate = currDate.Date.ToString("MM/dd/yyyy");

            existingVm.TimeSlotsWeekday = _getTimeSlotsProcess.Build(
                _callbackSetting.TimeSlotWeekdayStartTime,
                _callbackSetting.TimeSlotWeekdayEndTime);

            existingVm.TimeSlotsSaturday = _getTimeSlotsProcess.Build(
                _callbackSetting.TimeSlotSaturdayStartTime,
                _callbackSetting.TimeSlotSaturdayEndTime);

            existingVm.SlotsAvailableForCurrentDay =
                existingVm.TimeSlotsToday == null ? 0 : existingVm.TimeSlotsToday.Count;

            if (existingVm.SlotsAvailableForCurrentDay == 1)
            {
                var nextDay = currDate.Date.AddDays(1);

                if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    nextDay = currDate.Date.AddDays(1);
                }

                existingVm.FirstAvailableDate = nextDay.ToString("MM/dd/yyyy");

                if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                {
                    existingVm.TimeSlotsFirstAvailableDay = GetTimeSlots(
                        nextDay.Add(_callbackSetting.TimeSlotSaturdayStartTime),
                        nextDay.Add(_callbackSetting.TimeSlotSaturdayEndTime),
                        nextDay,
                        2);
                }
                else
                {
                    existingVm.TimeSlotsFirstAvailableDay = GetTimeSlots(
                        nextDay.Add(_callbackSetting.TimeSlotWeekdayStartTime),
                        nextDay.Add(_callbackSetting.TimeSlotWeekdayEndTime),
                        nextDay,
                        2);
                }
            }
            else
            {
                existingVm.TimeSlotsFirstAvailableDay = existingVm.TimeSlotsToday;
            }


        }

        private List<KeyValuePair<string, string>> GetAccountHoldersStatus()
        {
            var accountHolderStatuses = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Account Holder", "I am the account holder"),
                new KeyValuePair<string, string>("Third Party", "I am a third party")
            };

            return accountHolderStatuses;
        }

        private List<KeyValuePair<string, string>> GetCallbackTimeStatus()
        {
            DateTime currDate = DateTime.UtcNow.ToLocalTime();

            bool isCallmeNowRequired = false;
            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                isCallmeNowRequired = false;
            else if (currDate.DayOfWeek == DayOfWeek.Saturday)
                isCallmeNowRequired = currDate >= DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotSaturdayStartTime)
                    && currDate < DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotSaturdayEndTime).AddHours(-2);
            else
                isCallmeNowRequired = currDate >= DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotWeekdayStartTime)
                    && currDate < DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotWeekdayEndTime).AddHours(-1);

            var callbackTimeStatus = new List<KeyValuePair<string, string>>();

            if (isCallmeNowRequired)
                callbackTimeStatus.Add(new KeyValuePair<string, string>("CallMeNow", "Call me now"));

            callbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));

            return callbackTimeStatus;
        }

        private List<KeyValuePair<string, string>> GetTimeSlots(DateTime startTime, DateTime endTime,
            DateTime date, int durationInHours)
        {
            return _getTimeSlotsProcess.GetSlots(startTime, endTime, date, durationInHours);
        }

        private DateTime ToDateTime(TimeSpan time)
        {
            return DateTime.Now.Date.Add(time).ToLocalTime();
        }
    }
}
