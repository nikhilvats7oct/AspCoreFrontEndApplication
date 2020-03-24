using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.ViewModelBuilders;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class BuildCallbackVmServiceTest
    {
        private ILogger<BuildCallbackVmService> _logger;
        private readonly ILogger<GetTimeSlotsProcess> logger;
        private IGetTimeSlotsProcess _getTimeSlotsProcess;
        private CallbackSetting _callbackSetting;

        private BuildCallbackVmService _service;

        [TestInitialize]
        public void TestInitialise()
        {
            this._getTimeSlotsProcess = new GetTimeSlotsProcess(logger);
            this._callbackSetting = new CallbackSetting()
            {
                TimeSlotSaturdayStartTime = TimeSpan.Parse("08:00"),
                TimeSlotSaturdayEndTime = TimeSpan.Parse("14:00"),
                TimeSlotWeekdayStartTime = TimeSpan.Parse("08:00"),
                TimeSlotWeekdayEndTime = TimeSpan.Parse("20:00")
            };

            this._service = new BuildCallbackVmService(this._logger, this._getTimeSlotsProcess, this._callbackSetting);
        }

        [TestMethod]
        public void Create_CallbackVm_Object_Success_Sunday()
        {
            DateTime currDate = DateTime.UtcNow.ToLocalTime();
            if (currDate.DayOfWeek == DayOfWeek.Sunday)
            {
                var expectedCallbackTimeStatus = new List<KeyValuePair<string, string>>();
                expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));
                string expectedStr = JsonConvert.SerializeObject(expectedCallbackTimeStatus);

                var result = this._service.CreateNewCallbackVm();
                var resultStatus = result.CallbackOptions;
                string resultStr = JsonConvert.SerializeObject(resultStatus);

                Assert.AreEqual(expectedStr, resultStr);
            }
        }

        [TestMethod]
        public void Create_CallbackVm_Object_Success_Saturday()
        {
            DateTime currDate = DateTime.UtcNow.ToLocalTime();
            if (currDate.DayOfWeek == DayOfWeek.Saturday)
            {
                if (currDate >= DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotSaturdayStartTime)
                    && currDate < DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotSaturdayEndTime).AddHours(-2))
                {
                    var expectedCallbackTimeStatus = new List<KeyValuePair<string, string>>();
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("CallMeNow", "Call me now"));
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));
                    string expectedStr = JsonConvert.SerializeObject(expectedCallbackTimeStatus);

                    var result = this._service.CreateNewCallbackVm();
                    var resultStatus = result.CallbackOptions;
                    string resultStr = JsonConvert.SerializeObject(resultStatus);

                    Assert.AreEqual(expectedStr, resultStr);
                }
                else
                {
                    var expectedCallbackTimeStatus = new List<KeyValuePair<string, string>>();
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));
                    string expectedStr = JsonConvert.SerializeObject(expectedCallbackTimeStatus);

                    var result = this._service.CreateNewCallbackVm();
                    var resultStatus = result.CallbackOptions;
                    string resultStr = JsonConvert.SerializeObject(resultStatus);

                    Assert.AreEqual(expectedStr, resultStr);
                }
            }

        }

        [TestMethod]
        public void Create_CallbackVm_Object_Success_Weekdays()
        {
            DateTime currDate = DateTime.UtcNow.ToLocalTime();
            if (currDate.DayOfWeek == DayOfWeek.Sunday && currDate.DayOfWeek == DayOfWeek.Saturday)
            {
                if (currDate >= DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotWeekdayStartTime)
                    && currDate < DateTime.UtcNow.Date.Add(_callbackSetting.TimeSlotWeekdayEndTime).AddHours(-1))
                {
                    var expectedCallbackTimeStatus = new List<KeyValuePair<string, string>>();
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("CallMeNow", "Call me now"));
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));
                    string expectedStr = JsonConvert.SerializeObject(expectedCallbackTimeStatus);

                    var result = this._service.CreateNewCallbackVm();
                    var resultStatus = result.CallbackOptions;
                    string resultStr = JsonConvert.SerializeObject(resultStatus);

                    Assert.AreEqual(expectedStr, resultStr);
                }
                else
                {
                    var expectedCallbackTimeStatus = new List<KeyValuePair<string, string>>();
                    expectedCallbackTimeStatus.Add(new KeyValuePair<string, string>("Appointment", "Call me later"));
                    string expectedStr = JsonConvert.SerializeObject(expectedCallbackTimeStatus);

                    var result = this._service.CreateNewCallbackVm();
                    var resultStatus = result.CallbackOptions;
                    string resultStr = JsonConvert.SerializeObject(resultStatus);

                    Assert.AreEqual(expectedStr, resultStr);
                }
            }
        }



    }
}
