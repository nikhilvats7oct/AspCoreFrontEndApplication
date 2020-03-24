using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class DataProtectionVmTests
    {
        DataProtectionVmValidator _validator;
        private DataProtectionVm _model;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new DataProtectionVmValidator();

            // Contains sufficient setup for a model to be valid
            // Tests will change this to be invalid and create error states
            _model = new DataProtectionVm
            {
                // Below must be set to avoid other validation errors
                LowellReference = "123abc",

                Day = 15,
                Month = "May",
                Year = 1993,

                Postcode = "s4 7ul"

            };

            // Ensure valid
            Assert.IsTrue(_validator.Validate(_model).IsValid);
        }

        [TestMethod]
        [DataRow("0")] // minimal
        [DataRow("012345678")] // less than 10
        [DataRow("0123456789")] // exactly 10
        public void LowellReference_WhenValidateViaAttribute_WithValidEntry_UpToTenCharacters_Then_ReturnsNoError(
            string testLowellRef)
        {
            _model.LowellReference = testLowellRef;

            Assert.IsTrue(_model.LowellReference.Length <= 10);

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(true, result.IsValid);
                Assert.AreEqual(0, result.Errors.Count);
            }
        }

        [TestMethod]
        [DataRow("0123456789A")] // 11 chars
        [DataRow("0123456789AB")] // 12 chars
        public void LowellReference_WhenValidateViaAttribute_WithInvalidEntry_MoreThanTenCharacters_Then_ReturnsError(
            string testLowellRef)
        {
            _model.LowellReference = testLowellRef;

            Assert.IsTrue(_model.LowellReference.Length > 10);

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid Lowell reference", errors[0].ErrorMessage);
                }
            }
        }

        [TestMethod]
        [DataRow("abcde")] // alpha lowercase
        [DataRow("fghij")]
        [DataRow("klmno")]
        [DataRow("pqrst")]
        [DataRow("uvwxyz")]

        [DataRow("ABCDE")] // alpha uppercase
        [DataRow("FGHIJ")]
        [DataRow("KLMNO")]
        [DataRow("PQRST")]
        [DataRow("UVWXYZ")]

        [DataRow("0123456789")] // numeric

        [DataRow("KjAb8364")] // mixed
        public void LowellReference_WhenValidateViaAttribute_WithValidEntry_ContainsAlphanumeric_Then_ReturnsNoError(
            string testLowellRef)
        {
            _model.LowellReference = testLowellRef;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(true, result.IsValid);
                Assert.AreEqual(0, result.Errors.Count);
            }
        }

        [TestMethod]
        [DataRow(" ")] // Spaces not allowed
        [DataRow("  ")]
        [DataRow("    ")]

        [DataRow("!")] // Special characters (examples)
        [DataRow("#")]
        [DataRow("<")]
        [DataRow(">")]

        [DataRow("h[")] // Mixed
        [DataRow("]J")]
        [DataRow("A$B")]
        [DataRow("!1<")]
        public void LowellReference_WhenValidateViaAttribute_WithInvalidEntry_ContainsNonAlphanumeric_Then_ReturnsError(
            string testLowellRef)
        {
            _model.LowellReference = testLowellRef;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid Lowell reference", errors[0].ErrorMessage);
                }
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void LowellReference_WhenValidateViaAttribute_WithMissing_Then_ReturnsError(
            string testLowellRef)
        {
            _model.LowellReference = testLowellRef;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter your Lowell reference", errors[0].ErrorMessage);
                }
            }
        }

        [DataRow("CM2/0584")]
        [TestMethod]
        public void LowellReference_LowellReferenceWithForwardSlashValid(string lowellReference)
        {
            _model.LowellReference = lowellReference;

            ValidationResult result = _validator.Validate(_model);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [DataRow("CM2-0584")]
        [TestMethod]
        public void LowellReference_LowellReferenceWithHyphenValid(string lowellReference)
        {
            _model.LowellReference = lowellReference;

            ValidationResult result = _validator.Validate(_model);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        // No values
        [DataRow(null, null, null)]

        // Single value
        [DataRow(99, null, null)] // presence should be checked BEFORE checking for validity
        [DataRow(null, "anything", null)]
        [DataRow(null, null, 1000000)]

        // Two values
        [DataRow(99, "anything", null)]
        [DataRow(null, "anything", 1000000)]
        [DataRow(99, null, 1000000)]

        public void DateOfBirth_WhenSetViaComponents_WithInvalidDayMonthYear_Then_ReturnsNull(int? day, string month,
            int? year)
        {
            DataProtectionVm model = new DataProtectionVm()
            {
                Day = day,
                Month = month,
                Year = year
            };

            Assert.AreEqual(null, model.DateOfBirth);
        }

        [TestMethod]

        [DataRow(1, "JAN", 2018, 1)]
        [DataRow(15, "JAN", 2018, 1)]
        [DataRow(31, "JAN", 2018, 1)]
        [DataRow(31, " JAN ", 2018, 1)]

        [DataRow(1, "1", 2018, 1)]
        [DataRow(15, "1", 2018, 1)]
        [DataRow(31, "01", 2018, 1)]

        [DataRow(1, "FEB", 2018, 2)]
        [DataRow(15, "FEB", 2018, 2)]
        [DataRow(28, "FEB", 2018, 2)]

        [DataRow(1, "02", 2018, 2)]
        [DataRow(15, "2", 2018, 2)]
        [DataRow(28, "2", 2018, 2)]

        [DataRow(1, "NOV", 2018, 11)]
        [DataRow(15, "NOV", 2018, 11)]
        [DataRow(30, "NOV", 2018, 11)]

        [DataRow(1, "11", 2018, 11)]
        [DataRow(15, "11", 2018, 11)]
        [DataRow(30, "11", 2018, 11)]

        [DataRow(1, "DEC", 2018, 12)]
        [DataRow(15, "DEC", 2018, 12)]
        [DataRow(31, "DEC", 2018, 12)]

        [DataRow(1, "12", 2018, 12)]
        [DataRow(15, "12", 2018, 12)]
        [DataRow(31, "12", 2018, 12)]

        // Long names, max day (to ensure max characters test doesn't cause to break)
        [DataRow(31, "January", 2018, 1)]
        [DataRow(28, "February", 2018, 2)]
        [DataRow(31, "March", 2018, 3)]
        [DataRow(30, "April", 2018, 4)]
        [DataRow(31, "May", 2018, 5)]
        [DataRow(30, "June", 2018, 6)]
        [DataRow(31, "July", 2018, 7)]
        [DataRow(31, "August", 2018, 8)]
        [DataRow(30, "September", 2018, 9)]
        [DataRow(31, "October", 2018, 10)]
        [DataRow(30, "November", 2018, 11)]
        [DataRow(31, "December", 2018, 12)]

        // With spaces - still OK, so long as doesn't exceed 10 char limit
        [DataRow(31, " December ", 2018, 12)]

        public void DateOfBirth_WhenSetViaComponents_WithValidDayMonthYear_Then_ReturnsDateTime(int? day, string month,
            int? year, int expectedMonthNumber)
        {
            DataProtectionVm model = new DataProtectionVm()
            {
                Day = day,
                Month = month,
                Year = year
            };

            // To prevent warnings
            Assert.IsNotNull(day);
            Assert.IsNotNull(year);

            DateTime expected = new DateTime(year.Value, expectedMonthNumber, day.Value);
            Assert.AreEqual(expected, model.DateOfBirth);
        }

        [TestMethod]

        [DataRow(-1, "MAR", 2012)] // invalid day
        [DataRow(0, "MAR", 2012)] // invalid day
        [DataRow(32, "MAR", 2012)]
        [DataRow(99999, "MAR", 2012)]

        [DataRow(29, "FEB", 2018)] // invalid day of month
        [DataRow(31, "NOV", 2018)]

        [DataRow(15, "Woobar", 2012)] // invalid month
        [DataRow(15, "0", 2012)] // invalid month
        [DataRow(15, "13", 2012)] // invalid month

        [DataRow(15, "MAR", -1)] // invalid year
        [DataRow(15, "MAR", 99999)]

        public void DateOfBirth_WhenValidateViaAttribute_WithInvalidDate_Then_ReturnsError(int? day, string month,
            int? year)
        {
            // We're validating the property name on errors because it is overridden to
            // place errors against an alternative 'Date' property
            string errorPropertyName =
                Utilities.GetCheckedPropertyName(typeof(DataProtectionVm), "DateOfBirth");

            _model.Day = day;
            _model.Month = month;
            _model.Year = year;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid date, DD/MM/YYYY", errors[0].ErrorMessage);
                    Assert.AreEqual(errorPropertyName, errors[0].PropertyName);
                }
            }
        }

        [TestMethod]

        // No values
        [DataRow(null, null, null)]

        // Single value
        [DataRow(99, null, null)] // presence should be checked BEFORE checking for validity
        [DataRow(null, "anything", null)]
        [DataRow(null, null, 1000000)]

        // Two values
        [DataRow(99, "anything", null)]
        [DataRow(null, "anything", 1000000)]
        [DataRow(99, null, 1000000)]
        public void DateOfBirth_WhenValidateViaAttribute_WithMissingDate_Then_ReturnsError(int? day, string month,
            int? year)
        {
            // We're validating the property name on errors because it is overridden to
            // place errors against an alternative 'Date' property
            string errorPropertyName =
                Utilities.GetCheckedPropertyName(typeof(DataProtectionVm), "DateOfBirth");

            _model.Day = day;
            _model.Month = month;
            _model.Year = year;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter your date of birth, DD/MM/YYYY", errors[0].ErrorMessage);
                    Assert.AreEqual(errorPropertyName, errors[0].PropertyName);
                }
            }
        }

        [TestMethod]

        [DataRow(1, "JAN", 2018)]
        [DataRow(15, "JAN", 2018)]
        [DataRow(31, "JAN", 2018)]

        [DataRow(1, "1", 2018)]
        [DataRow(15, "1", 2018)]
        [DataRow(31, "01", 2018)]

        [DataRow(1, "FEB", 2018)]
        [DataRow(15, "FEB", 2018)]
        [DataRow(28, "FEB", 2018)]

        [DataRow(1, "02", 2018)]
        [DataRow(15, "2", 2018)]
        [DataRow(28, "2", 2018)]

        [DataRow(1, "NOV", 2018)]
        [DataRow(15, "NOV", 2018)]
        [DataRow(30, "NOV", 2018)]

        [DataRow(1, "11", 2018)]
        [DataRow(15, "11", 2018)]
        [DataRow(30, "11", 2018)]

        [DataRow(1, "DEC", 2018)]
        [DataRow(15, "DEC", 2018)]
        [DataRow(31, "DEC", 2018)]

        [DataRow(1, "12", 2018)]
        [DataRow(15, "12", 2018)]
        [DataRow(31, "12", 2018)]

        // Long names, max day (to ensure max characters test doesn't cause to break)
        [DataRow(31, "January", 2018)]
        [DataRow(28, "February", 2018)]
        [DataRow(31, "March", 2018)]
        [DataRow(30, "April", 2018)]
        [DataRow(31, "May", 2018)]
        [DataRow(30, "June", 2018)]
        [DataRow(31, "July", 2018)]
        [DataRow(31, "August", 2018)]
        [DataRow(30, "September", 2018)]
        [DataRow(31, "October", 2018)]
        [DataRow(30, "November", 2018)]
        [DataRow(31, "December", 2018)]

        // TODO: lots more

        public void DateOfBirth_WhenValidateViaAttribute_WithValidDate_Then_ReturnsNoError(int? day, string month,
            int? year)
        {
            _model.Day = day;
            _model.Month = month;
            _model.Year = year;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(true, result.IsValid);
                Assert.AreEqual(0, result.Errors.Count);
            }
        }

        [TestMethod]

        [DataRow("  March    ")] // 11 chars - caused by spaces
        [DataRow("  March     ")] // 12 chars - caused by spaces

        [DataRow("0123456789A")] // 11 characters - invalid and too long
        [DataRow("012345678912")] // 12 characters

        public void DateOfBirth_WhenValidateViaAttribute_WithMonthMoreThanTenCharacters_Then_ReturnsError(string month)
        {
            // We're validating the property name on errors because it is overridden to
            // place errors against an alternative 'Date' property
            string errorPropertyName =
                Utilities.GetCheckedPropertyName(typeof(DataProtectionVm), "DateOfBirth");

            _model.Day = 15;
            _model.Month = month; // only care about month
            _model.Year = 2018;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid date, DD/MM/YYYY", errors[0].ErrorMessage);
                    Assert.AreEqual(errorPropertyName, errors[0].PropertyName);
                }
            }
        }

        [Ignore("Broken Test")]
        [TestMethod]

        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("   ")]

        public void PostCode_WhenValidateViaAttribute_WithMissingPostCode_Then_ReturnsError(string testPostCode)
        {
            _model.Postcode = testPostCode;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter your postcode", errors[0].ErrorMessage);
                }
            }
        }

        [Ignore("Broken Test")]
        [TestMethod]

        [DataRow("s4 7ul")]
        [DataRow(" s4 7ul ")]
        [DataRow("xxxxxxxxxx")] // up to 10 chars

        public void PostCode_WhenValidateViaAttribute_WithValidPostCode_Then_ReturnsNoError(string testPostCode)
        {
            _model.Postcode = testPostCode;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(true, result.IsValid);
                Assert.AreEqual(0, result.Errors.Count);
            }
        }

        [TestMethod]

        [DataRow("0123456789A")] // 11 chars
        [DataRow(" 0123456789")]
        [DataRow("0123456789 ")]

        [DataRow("abcdefghijkl")] // 12 chars

        public void PostCode_WhenValidateViaAttribute_WithMoreThanTenCharacters_Then_ReturnsError(string testPostCode)
        {
            // Real postcodes are 6-8 characters, allowing for space
            // Maximum is 10, so as to give wiggle room

            _model.Postcode = testPostCode;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid postcode", errors[0].ErrorMessage);
                }
            }
        }

        [Ignore("Broken Test")]
        [TestMethod]
        [DataRow("!")] // Special characters (examples)
        [DataRow("#")]
        [DataRow("<")]
        [DataRow(">")]

        [DataRow("h[")] // Mixed
        [DataRow(" ]J")]
        [DataRow("A$B ")]
        [DataRow("!1<")]
        [DataRow("S4 7UL!")]
        public void PostCode_WhenValidateViaAttribute_WithInvalidEntry_ContainsNonAlphanumeric_Then_ReturnsError(
            string testPostCode)
        {
            _model.Postcode = testPostCode;

            {
                ValidationResult result = _validator.Validate(_model);
                Assert.AreEqual(false, result.IsValid);
                {
                    var errors = result.Errors;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Please enter a valid postcode", errors[0].ErrorMessage);
                }
            }
        }

    }
}
