using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class CallbackValidatorTest
    {
        private CallbackVmValidator _validator;
        private CallbackVm _callbackVm;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new CallbackVmValidator();
            var date = DateTime.Now.AddYears(-24);

            _callbackVm = new CallbackVm()
            {
                LowellReferenceNumber = "A100432489",
                AccountHolderStatus = "accountholder",
                FullName = "John Smith",
                PreferredTelephoneNumber = "+447123456789",
                TimeSlot = "10:00 AM - 12:00 PM",
                TimeSlotsFirstAvailableDay = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("a", "a") }
            };
        }

        [TestMethod]
        public void ValidStateTest()
        {
            ValidationResult result = _validator.Validate(_callbackVm);


            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Failed_On_Empty_FullName()
        {
            _callbackVm.FullName = null;

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyFullName, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_FullName_With_Numeric_Characters()
        {
            _callbackVm.FullName = "John 123";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validation_Failed_On_FullName_With_Special_Characters()
        {
            _callbackVm.FullName = "John +=";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validation_Failed_On_Empty_Holder_Status()
        {
            _callbackVm.AccountHolderStatus = string.Empty;

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyAccountHolderStatus, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Contains_Space()
        {
            _callbackVm.LowellReferenceNumber = "A100 4323B";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Contains_Special_Characters()
        {
            _callbackVm.LowellReferenceNumber = "A100#4323B";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Validation_Failed_On_Account_Reference_Exceeds_Maximum_Characters()
        {
            _callbackVm.LowellReferenceNumber = "A100C4323B123";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        //[TestMethod]
        public void Validation_Failed_On_Account_Reference_LessThan_Minimum_Characters()
        {
            _callbackVm.LowellReferenceNumber = "A100C";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.EmptyOrInvalidAccountReference, result.Errors[0].ErrorMessage);
        }

        //Preferred telephone number
        //Number can be no longer than 17 characters if starting with +44
        //Number can be no longer than 12 characters if starting with 0 i.e 11 numbers plus a space
        //Number can include digits(0-9), single spaces, and the '+' symbol as the first character
        //contains at least 11 digits only numeric characters and spaces

        [TestMethod]
        public void Validation_Success_On_Mobile_Start_With_zero()
        {
            _callbackVm.PreferredTelephoneNumber = "01234567890";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validation_Success_On_Mobile_Start_With_Plus44()
        {
            _callbackVm.PreferredTelephoneNumber = "+441234567890";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        //Todo: need to be confirmed about space
        //[TestMethod]
        public void Validation_Success_On_Mobile_Start_With_0_Contains_Single_Space()
        {
            _callbackVm.PreferredTelephoneNumber = "0 4123123456";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        //Todo: need to be confirmed about space
        [TestMethod]
        public void Validation_Success_On_Mobile_Start_With_Plus44_Contains_Single_Space()
        {
            _callbackVm.PreferredTelephoneNumber = "+44 1234567890";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        // [TestMethod]
        public void Validation_Success_On_Mobile_start_without_zero()
        {
            _callbackVm.PreferredTelephoneNumber = "4123123456";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        // [TestMethod]
        public void Validation_Failed_On_Mobile_AtLeast_11_digits()
        {
            _callbackVm.PreferredTelephoneNumber = "23456789";

            ValidationResult result = _validator.Validate(_callbackVm);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.Callback.InvalidPreferredTelephoneNumber, result.Errors[0].ErrorMessage);
        }

    }
}
