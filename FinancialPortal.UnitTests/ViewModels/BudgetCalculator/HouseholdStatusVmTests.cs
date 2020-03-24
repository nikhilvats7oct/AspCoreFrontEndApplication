using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FinancialPortal.Web.Validation.BudgetCalculator;

namespace FinancialPortal.UnitTests.ViewModels.BudgetCalculator
{
    [TestClass]
    public class HouseholdStatusVmTests
    {
        private HouseholdStatusVmValidator _validator;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new HouseholdStatusVmValidator();
        }

        [TestMethod]
        public void HouseholdStatusVm_WithCircumstanceAndHousingStatusSet_NoValidationErrors()
        {
            var model = new HouseholdStatusVm()
            {
                EmploymentStatus = "Employed",
                HousingStatus = "Mortgage"
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        [TestMethod]
        public void HouseholdStatusVm_WithCircumstanceAndHousingStatusNotSet_FailsValidation()
        {
            var model = new HouseholdStatusVm()
            {
                EmploymentStatus = "Please select",
                HousingStatus = "Please select"
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid == false);
            Assert.AreEqual(result.Errors.Count, 2);
        }

        [TestMethod]
        public void HouseholdStatusVm_WithEitherCircumstanceorHousingStatusNotSet_FailsValidation()
        {
            var householdDetailsList = new List<HouseholdStatusVm>()
            {
               new HouseholdStatusVm()
               {
                    EmploymentStatus = "Employed",
                    HousingStatus = "Please select"
               },
               new HouseholdStatusVm()
               {
                    EmploymentStatus = "Please select",
                    HousingStatus = "Mortgage"
               }
            };

            foreach (var model in householdDetailsList)
            {
                var result = _validator.Validate(model);

                Assert.IsTrue(result.IsValid == false);
                Assert.AreEqual(result.Errors.Count, 1);
            }
        }
    }
}
