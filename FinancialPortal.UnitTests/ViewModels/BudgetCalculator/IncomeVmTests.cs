using System.Diagnostics.CodeAnalysis;
using FinancialPortal.Web.Validation.BudgetCalculator;
using FinancialPortal.Web.ViewModels;
using FinancialPortal.Web.ViewModels.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.ViewModels.BudgetCalculator
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class IncomeVmTests
    {
        private IncomeVmValidator _validator;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new IncomeVmValidator();
        }

        [TestMethod]
        public void Income_AllIncome_AmountSetToValue0_And_FrequencyNotSet_FailsValidation()
        {
            var model = new IncomeVm()
            {
                BenefitsAndTaxCredits = new IncomeSourceVm()
                {
                    Amount = 0
                },
                Earning = new IncomeSourceVm()
                {
                    Amount = 0
                },
                Other = new IncomeSourceVm()
                {
                    Amount = 0
                },
                Pension = new IncomeSourceVm()
                {
                    Amount = 0
                }
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid == false);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("All income fields cannot be zero", result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Income_AllIncome_AmounthasValue_And_FrequencySet_NoValidationErrors()
        {
            var model = new IncomeVm()
            {
                BenefitsAndTaxCredits = new IncomeSourceVm()
                {
                    Amount = 100,
                    Frequency = "Monthly"
                },
                Earning = new IncomeSourceVm()
                {
                    Amount = 100,
                    Frequency = "Monthly"
                },
                Other = new IncomeSourceVm()
                {
                    Amount = 100,
                    Frequency = "Monthly"
                },
                Pension = new IncomeSourceVm()
                {
                    Amount = 100,
                    Frequency = "Monthly"
                }
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 0);
        }
    }
}
