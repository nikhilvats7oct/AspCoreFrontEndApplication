using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class ContactUsValidatorTests
    {
        private ContactUsVmValidator _validator;
        private ContactUsVm _contactUsVm;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new ContactUsVmValidator();
            var date = DateTime.Now.AddYears(-24);

            _contactUsVm = new ContactUsVm()
            {
                Day = date.Day,
                Month = date.ToString("MMMM", CultureInfo.InvariantCulture),
                Year = date.Year,
                LowellReferenceNumber = "A100432489",
                AccountHolderStatus = "accountholder",
                ContactUsEmailAddress = "john.smith@gmail.com",
                FirstLineOfAddress = "32 Leeds Road",
                QueryTopic = "Website Issue",
                FullName = "John Smith",
                Postcode = "LS13 6YG",
                MessageContent = "Website Issues"
            };
        }

        [TestMethod]
        public void ValidStateTest()
        {
            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public void Validation_Failed_On_Empty_QuerySelection()
        {
            _contactUsVm.QueryTopic = string.Empty;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyQueryArea, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Null_QuerySelection()
        {
            _contactUsVm.QueryTopic = null;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyQueryArea, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_WhiteSpaces_QuerySelection()
        {
            _contactUsVm.QueryTopic = "  ";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyQueryArea, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Empty_FullName()
        {
            _contactUsVm.FullName = null;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyFullName, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_FullName_With_Numeric_Characters()
        {
            _contactUsVm.FullName = "John 123";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyFullName, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_FullName_With_Special_Characters()
        {
            _contactUsVm.FullName = "John +=";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyFullName, result.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Validation_Failed_On_FullName_With_Exceed_Maximum_Length()
        {
            _contactUsVm.FullName = new string('a', 101);

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyFullName, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Empty_Holder_Status()
        {
            _contactUsVm.AccountHolderStatus = string.Empty;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyAccountHolderStatus, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Empty_Email()
        {
            _contactUsVm.ContactUsEmailAddress = string.Empty;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Email_With_SpecialCharacters()
        {
            _contactUsVm.ContactUsEmailAddress = "abc#12@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Email_StartsWith_dot()
        {
            _contactUsVm.ContactUsEmailAddress = ".abc@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Email_EndsWith_dot()
        {
            _contactUsVm.ContactUsEmailAddress = "abc@gmail.com.";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Two_Consecutive_Dots()
        {
            _contactUsVm.ContactUsEmailAddress = "abc..ee@gmail.com.";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Success_On_Email_Include_Dots()
        {
            _contactUsVm.ContactUsEmailAddress = "john.smith@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Success_On_Email_Include_Underscore()
        {
            _contactUsVm.ContactUsEmailAddress = "john_smith@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Success_On_Email_Include_Hyphen()
        {
            _contactUsVm.ContactUsEmailAddress = "john_smith@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Success_On_Email_Include_Numberic_Characters()
        {
            _contactUsVm.ContactUsEmailAddress = "john_smith123@gmail.com";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Contains_Space()
        {
            _contactUsVm.LowellReferenceNumber = "A100 4323B";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Contains_Special_Characters()
        {
            _contactUsVm.LowellReferenceNumber = "A100#4323B";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Exceeds_Maximum_Characters()
        {
            _contactUsVm.LowellReferenceNumber = "A100C4323B123";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_LessThan_Minimum_Characters()
        {
            _contactUsVm.LowellReferenceNumber = "A100C";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_On_Sepcial_Characters()
        {
            _contactUsVm.FirstLineOfAddress = "A100#4323B";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidFirstLineOfAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_On_Empty()
        {
            _contactUsVm.FirstLineOfAddress = "";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidFirstLineOfAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_On_Exceed_Maximum_Length()
        {
            _contactUsVm.FirstLineOfAddress = new string('f', 201);

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidFirstLineOfAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_Invalid_Postcode_Numbers()
        {
            _contactUsVm.Postcode = "123";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidPostcode, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_Invalid_Postcode_Characters()
        {
            _contactUsVm.Postcode = "abc";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidPostcode, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_FirstLineAddress_Failed_Empty_Postcode()
        {
            _contactUsVm.Postcode = "";

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyOrInvalidPostcode, result.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Validation_On_Failed_Empty_FreeText()
        {
            _contactUsVm.MessageContent = string.Empty;

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_On_Failed_FreeText_LongerThan_MaximumLength()
        {
            _contactUsVm.MessageContent = new string('g', 501);

            ValidationResult result = _validator.Validate(_contactUsVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ContactUs.EmptyMessage, result.Errors[0].ErrorMessage);
        }
    }
}
