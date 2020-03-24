using System;
using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class GetTimeSlotsProcessTest
    {
        private Mock<ILogger<GetTimeSlotsProcess>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GetTimeSlotsProcess>>();
        }
        
        [DataTestMethod]
        //Booking time before office hours, all solts avaliable
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 00:00", 2, 6, DisplayName = "At 00:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 00:20", 2, 6, DisplayName = "At 00:20")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 01:50", 2, 6, DisplayName = "At 01:50")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 02:00", 2, 6, DisplayName = "At 02:00")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 03:33", 2, 6, DisplayName = "At 03:33")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 04:22", 2, 6, DisplayName = "At 04:22")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 05:18", 2, 6, DisplayName = "At 05:18")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 06:00", 2, 6, DisplayName = "At 06:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 07:59", 2, 6, DisplayName = "At 07:59")]

        //Booking time within working hours
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 08:00", 2, 5, DisplayName = "At 08:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 08:15", 2, 5, DisplayName = "At 08:15")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 09:00", 2, 5, DisplayName = "At 09:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 09:30", 2, 5, DisplayName = "At 09:30")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 10:00", 2, 4, DisplayName = "At 10:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 10:45", 2, 4, DisplayName = "At 10:45")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 11:00", 2, 4, DisplayName = "At 11:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 11:05", 2, 4, DisplayName = "At 11:05")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 12:00", 2, 3, DisplayName = "At 12:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 12:20", 2, 3, DisplayName = "At 12:20")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 13:00", 2, 3, DisplayName = "At 13:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 13:30", 2, 3, DisplayName = "At 13:30")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 14:00", 2, 2, DisplayName = "At 14:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 14:40", 2, 2, DisplayName = "At 14:40")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 15:00", 2, 2, DisplayName = "At 15:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 15:50", 2, 2, DisplayName = "At 15:50")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 15:59", 2, 2, DisplayName = "At 15:59")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 16:00", 2, 1, DisplayName = "At 16:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 16:03", 2, 1, DisplayName = "At 16:03")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 17:00", 2, 1, DisplayName = "At 17:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 17:23", 2, 1, DisplayName = "At 17:23")]

        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 18:00", 2, 0, DisplayName = "At 18:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 18:43", 2, 0, DisplayName = "At 18:43")]

        //Booking time within the last slot
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 19:00", 2, 0, DisplayName = "At 19:00")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 19:50", 2, 0, DisplayName = "At 19:50")]

        //Booking time after the last slot
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 20:20", 2, 0, DisplayName = "At 20:20")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 21:47", 2, 0, DisplayName = "At 21:47")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 22:22", 2, 0, DisplayName = "At 22:22")]
        [DataRow("2019-09-19 08:00", "2019-09-19 20:00", "2019-09-19 23:17", 2, 0, DisplayName = "At 23:17")]

        //UTC Time Test
        [DataRow("2019-11-11 08:00", "2019-11-11 20:00", "2019-11-11 23:17", 2, 0, DisplayName = "At 23:17")]

        public void GetSlots(string start, string end, string current, int slotDuration, int expected)
        {
            var process = new GetTimeSlotsProcess(_mockLogger.Object);

            var startDateTime = DateTime.Parse(start);
            var endDateTime = DateTime.Parse(end);
            var currentDateTime = DateTime.Parse(current);

            var result = process.GetSlots(startDateTime,endDateTime, currentDateTime, slotDuration);

            Assert.AreEqual(result[0].Key, null);
            Assert.AreEqual(result[0].Value, "Time slot");

            result.Remove(new KeyValuePair<string, string>(null, "Time slot"));

            Assert.AreEqual(result.Count, expected);
        }

        [TestMethod]
        public void Build_WhenCall_Returns_Timeslots_For_Today()
        {
            GetTimeSlotsProcess process = new GetTimeSlotsProcess(_mockLogger.Object);

            DateTime currDate = DateTime.UtcNow;

            TimeSpan CallbackTimeSlotWeekdayStartTime = TimeSpan.Parse("08:00");
            TimeSpan CallbackTimeSlotWeekdayEndTime = TimeSpan.Parse("20:00");

            TimeSpan CallbackTimeSlotSaturdayStartTime = TimeSpan.Parse("08:00");
            TimeSpan CallbackTimeSlotSaturdayEndTime = TimeSpan.Parse("14:00");
            
            if (currDate.DayOfWeek == DayOfWeek.Saturday)
            {
                List<KeyValuePair<string, string>> list = process.GetSlots(ToDateTime(CallbackTimeSlotSaturdayStartTime), ToDateTime(CallbackTimeSlotSaturdayEndTime), currDate, 2);
                Assert.IsNotNull(list);
            }
            else
            {
                List<KeyValuePair<string, string>> list = process.GetSlots(ToDateTime(CallbackTimeSlotWeekdayStartTime), ToDateTime(CallbackTimeSlotWeekdayEndTime), currDate, 2);
                Assert.IsNotNull(list);
            }            
        }

        [TestMethod]
        public void Build_WhenCall_Returns_Timeslots_For_Weekdays()
        {
            GetTimeSlotsProcess process = new GetTimeSlotsProcess(_mockLogger.Object);

            TimeSpan CallbackTimeSlotWeekdayStartTime = TimeSpan.Parse("08:00");
            TimeSpan CallbackTimeSlotWeekdayEndTime = TimeSpan.Parse("20:00");

            List<KeyValuePair<string, string>> list = process.Build(CallbackTimeSlotWeekdayStartTime, CallbackTimeSlotWeekdayEndTime);
            Assert.AreEqual(7, list.Count);

            AssertKeyValuePair(null, "Time slot", list[0]);
            AssertKeyValuePair("08:00 AM - 10:00 AM", "08:00 AM - 10:00 AM", list[1]);
            AssertKeyValuePair("10:00 AM - 12:00 PM", "10:00 AM - 12:00 PM", list[2]);
            AssertKeyValuePair("12:00 PM - 02:00 PM", "12:00 PM - 02:00 PM", list[3]);
            AssertKeyValuePair("02:00 PM - 04:00 PM", "02:00 PM - 04:00 PM", list[4]);
            AssertKeyValuePair("04:00 PM - 06:00 PM", "04:00 PM - 06:00 PM", list[5]);
            AssertKeyValuePair("06:00 PM - 08:00 PM", "06:00 PM - 08:00 PM", list[6]);
        }

        [TestMethod]
        public void Build_WhenCall_Returns_Timeslots_For_Saturday()
        {
            GetTimeSlotsProcess process = new GetTimeSlotsProcess(_mockLogger.Object);

            TimeSpan CallbackTimeSlotSaturdayStartTime = TimeSpan.Parse("08:00");
            TimeSpan CallbackTimeSlotSaturdayEndTime = TimeSpan.Parse("14:00");

            List<KeyValuePair<string, string>> list = process.Build(CallbackTimeSlotSaturdayStartTime, CallbackTimeSlotSaturdayEndTime);
            Assert.AreEqual(4, list.Count);

            AssertKeyValuePair(null, "Time slot", list[0]);
            AssertKeyValuePair("08:00 AM - 10:00 AM", "08:00 AM - 10:00 AM", list[1]);
            AssertKeyValuePair("10:00 AM - 12:00 PM", "10:00 AM - 12:00 PM", list[2]);
            AssertKeyValuePair("12:00 PM - 02:00 PM", "12:00 PM - 02:00 PM", list[3]);
        }

        [TestMethod]
        public void Build_WhenCall_Returns_Timeslots_For_Sunday()
        {
            GetTimeSlotsProcess process = new GetTimeSlotsProcess(_mockLogger.Object);

            TimeSpan CallbackTimeSlotStartTime = TimeSpan.Parse("14:00");
            TimeSpan CallbackTimeSlotEndTime = TimeSpan.Parse("08:00");

            List<KeyValuePair<string, string>> list = process.Build(CallbackTimeSlotStartTime, CallbackTimeSlotEndTime);
            Assert.AreEqual(1, list.Count);
        }

        void AssertKeyValuePair(string expectedKey, string expectedValue, KeyValuePair<string, string> pair)
        {
            Assert.AreEqual(expectedKey, pair.Key);
            Assert.AreEqual(expectedValue, pair.Value);
        }

        private DateTime ToDateTime(TimeSpan time)
        {
            return DateTime.Now.Date.Add(time).ToLocalTime();
        }
    }
}
