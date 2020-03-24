using System;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class BuildDataProtectionDtoProcessTests
    {
        private Mock<ILogger<BuildDataProtectionDtoProcess>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<BuildDataProtectionDtoProcess>>();
        }

        [TestMethod]
        public void Build_WhenCallWithValidModel_ConvertsIntoDto()
        {
            DataProtectionVm vm = new DataProtectionVm()
            {
                LowellReference = "123abc",
                Day = 17,
                Month = "September",
                Year = 2018,
                Postcode = "S4 7UL"
            };

            BuildDataProtectionDtoProcess process = new BuildDataProtectionDtoProcess(_mockLogger.Object);
            DataProtectionDto dto = process.Build(vm);

            Assert.AreEqual("123abc", dto.LowellReference);
            Assert.AreEqual(new DateTime(2018, 9, 17), dto.DateOfBirth);
            Assert.AreEqual("S4 7UL", dto.Postcode);
        }

        [TestMethod]
        public void Build_WhenCallWithValidModel_ConvertsIntoDto_AndMakesPostCodeUpperCase()
        {
            // Converts postcode to upper case = to ensure correct for CaseFlow
            // Not good enough to just do on client as per user requirements

            DataProtectionVm vm = new DataProtectionVm()
            {
                LowellReference = "123abc",
                Day = 17,
                Month = "September",
                Year = 2018,
                Postcode = "s4 7ul"
            };

            BuildDataProtectionDtoProcess process = new BuildDataProtectionDtoProcess(_mockLogger.Object);
            DataProtectionDto dto = process.Build(vm);

            Assert.AreEqual("123abc", dto.LowellReference);
            Assert.AreEqual(new DateTime(2018, 9, 17), dto.DateOfBirth);
            Assert.AreEqual("S4 7UL", dto.Postcode);
        }
    }
}
