using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FinancialPortal.UnitTests
{
    [ExcludeFromCodeCoverage]
    public static class Utilities
    {
        // This ensures that a property name exists on a model and returns its checked name for use in asserts
        internal static string GetCheckedPropertyName(Type modelType, string propertyName)
        {
            PropertyInfo property = modelType.GetProperty(propertyName);
            Assert.IsNotNull(property, $"Property '{propertyName}' does not exist on model {modelType.Name}");
            return property.Name;
        }

        /// <summary>
        /// Provides a deep, independent copy of the given argument
        /// </summary>
        internal static T DeepCopy<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Recursively compares each propery on the given objects for equality. Returns true if the objects are identical.
        /// Throws an exception if any property is found not to be equal with a message for unit testing purposes
        /// </summary>
        public static Boolean DeepCompare(Object object_1, Object object_2, Int32 datetime_delta_ms = 0)
        {
            return DeepCompare(object_1, object_2, new List<String>() { }, datetime_delta_ms);
        }

        /// <summary>
        /// Recursively compares each propery on the given objects for equality. Returns true if the objects are identical.
        /// Throws an exception if any property is found not to be equal with a message for unit testing purposes
        /// </summary>
        internal static bool DeepCompare(object object1, object object2, List<string> properties_to_ignore, int datetime_delta_ms)
        {
            if (object1 == null && object2 == null) { return true; }

            //check if both objects are not null before starting comparing children
            if (object1 != null && object2 != null)
            {
                if (object1.GetType() != object2.GetType()) 
                {
                    throw new Exception("The objects compared are of different types"); 
                }

                List<PropertyInfo> properties = new List<PropertyInfo>();
                properties.AddRange(object1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance));
                properties.AddRange(object1.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance));

                List<FieldInfo> fields = new List<FieldInfo>();
                fields.AddRange(object1.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance));
                fields.AddRange(object1.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance));

                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanRead || properties_to_ignore.Contains(property.Name))
                    {
                        continue;
                    }

                    Object value_1 = property.GetValue(object1, null);
                    Object value_2 = property.GetValue(object2, null);

                    if (datetime_delta_ms > 0 && (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime)))
                    {
                        if (!CompareDateTimes((DateTime?)value_1, (DateTime?)value_2, datetime_delta_ms))
                        {
                            throw new Exception($"Property {property.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (IsAssignableFrom(property.PropertyType) || property.PropertyType.IsPrimitive || property.PropertyType.IsValueType)
                    {
                        //If the objects are primitive types such as (integer, string etc) that implement IComparable, we can just directly try and compare the value     
                        if (!CompareValues(value_1, value_2))
                        {
                            throw new Exception($"Property {property.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (IsEnumerableType(property.PropertyType)) //if the property is a collection (or something that implements IEnumerable) we have to iterate through all items and compare values
                    {
                        if (!CompareEnumerations(value_1, value_2))
                        {
                            throw new Exception($"Property {property.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (property.PropertyType.IsClass) //if it is a class object, call the same function recursively again
                    {
                        try
                        {
                            DeepCompare(property.GetValue(object1, null), property.GetValue(object2, null), properties_to_ignore, datetime_delta_ms);
                        }
                        catch (Exception e) 
                        {
                            throw new Exception($"Property {property.Name} not equal. Object 1 {value_1}, Object 2 {value_2}", e);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (FieldInfo field in fields)
                {
                    if (properties_to_ignore.Contains(field.Name))
                    {
                        continue;
                    }

                    Object value_1 = field.GetValue(object1);
                    Object value_2 = field.GetValue(object2);

                    if (datetime_delta_ms > 0 && (field.FieldType == typeof(DateTime?) || field.FieldType == typeof(DateTime)))
                    {
                        if (!CompareDateTimes((DateTime?)value_1, (DateTime?)value_2, datetime_delta_ms))
                        {
                            throw new Exception($"Property {field.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (IsAssignableFrom(field.FieldType) || field.FieldType.IsPrimitive || field.FieldType.IsValueType)
                    {
                        //If the objects are primitive types such as (integer, string etc) that implement IComparable, we can just directly try and compare the value     
                        if (!CompareValues(value_1, value_2))
                        {
                            throw new Exception($"Property {field.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (IsEnumerableType(field.FieldType)) //if the property is a collection (or something that implements IEnumerable) we have to iterate through all items and compare values
                    {
                        if (!CompareEnumerations(value_1, value_2))
                        {
                            throw new Exception($"Property {field.Name} not equal. Object 1 {value_1}, Object 2 {value_2}");
                        }
                    }
                    else if (field.FieldType.IsClass) //if it is a class object, call the same function recursively again
                    {
                        try
                        {
                            DeepCompare(field.GetValue(object1), field.GetValue(object2), properties_to_ignore, datetime_delta_ms);
                        }
                        catch (Exception e) 
                        {
                            throw new Exception($"Property {field.Name} not equal. Object 1 {value_1}, Object 2 {value_2}", e);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new Exception("One of the objects compared is null the other is not");
            }

            return true;
        }

        private static Boolean CompareDateTimes(DateTime? datetime1, DateTime? datetime2, Int32 datetime_delta_ms)
        {
            if (datetime1 == null && datetime2 == null) { return true; }
            if (datetime1 == null || datetime2 == null) { return false; }

            return Math.Abs((datetime1.Value.Subtract(datetime2.Value)).TotalMilliseconds) <= datetime_delta_ms;
        }

        private static Boolean IsAssignableFrom(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type);
        }

        private static Boolean CompareValues(Object value_1, Object value_2)
        {
            IComparable selfValueComparer = value_1 as IComparable;

            if (value_1 == null && value_2 != null || value_1 != null && value_2 == null)
            {
                return false;
            }
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value_2) != 0)
            {
                return false;
            }
            else if (!object.Equals(value_1, value_2))
            {
                return false;
            }

            return true;
        }

        private static Boolean IsEnumerableType(Type type)
        {
            return (typeof(IEnumerable).IsAssignableFrom(type));
        }

        private static Boolean CompareEnumerations(Object value_1, Object value_2)
        {
            if (value_1 == null && value_2 != null || value_1 != null && value_2 == null)
            {
                return false;
            }
            else if (value_1 != null && value_2 != null)
            {
                IEnumerable<Object> enum_value_1 = ((IEnumerable)value_1).Cast<Object>();
                IEnumerable<Object> enum_value_2 = ((IEnumerable)value_2).Cast<Object>();

                if (enum_value_1.Count() != enum_value_2.Count())
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < enum_value_1.Count(); i++)
                    {
                        Object enum_value_1_item = enum_value_1.ElementAt(i);
                        Object enum_value_2_item = enum_value_2.ElementAt(i);

                        Type enum_value_1_item_type = enum_value_1_item.GetType();

                        if (IsAssignableFrom(enum_value_1_item_type) || enum_value_1_item_type.IsPrimitive || enum_value_1_item_type.IsValueType)
                        {
                            if (!CompareValues(enum_value_1_item, enum_value_2_item))
                            {
                                return false;
                            }
                        }
                        else if (!DeepCompare(enum_value_1_item, enum_value_2_item))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static IncomeAndExpenditure CreateDefaultTestIAndE() 
        {
            return new IncomeAndExpenditure() 
            {
                Mortgage = Int32.MaxValue,
                MortgageArrears = Int32.MaxValue,
                MortgageFrequency = "test",
                Rent = Int32.MaxValue,
                RentalArrears = Int32.MaxValue,
                RentFrequency = "test",
                CCJs = Int32.MaxValue,
                CCJsArrears = Int32.MaxValue,
                CCJsFrequency = "test",
                ChildMaintenance = Int32.MaxValue,
                ChildMaintenanceArrears = Int32.MaxValue,
                ChildMaintenanceFrequency = "test",
                Children16to18 = Int32.MaxValue,
                ChildrenUnder16 = Int32.MaxValue,
                Gas = Int32.MaxValue,
                GasArrears = Int32.MaxValue,
                GasFrequency = "test",
                HasArrears = true,
                Electricity = Int32.MaxValue,
                ElectricityArrears = Int32.MaxValue,
                ElectricityFrequency = "test",
                AdultsInHousehold = Int32.MaxValue,
                Water = Int32.MaxValue,
                WaterArrears = Int32.MaxValue,
                WaterFrequency = "test",
                BenefitsTotal = Int32.MaxValue,
                BenefitsTotalFrequency = "test",
                CouncilTax = Int32.MaxValue,
                CouncilTaxArrears = Int32.MaxValue,
                CouncilTaxFrequency = "test",
                CourtFines = Int32.MaxValue,
                CourtFinesArrears = Int32.MaxValue,
                CourtFinesFrequency = "test",
                Created = DateTime.MaxValue,
                DisposableIncome = Int32.MaxValue,
                EarningsTotal = Int32.MaxValue,
                EarningsTotalFrequency = "test",
                ExpenditureTotal = Int32.MaxValue,
                EmploymentStatus = "test",
                OtherExpenditure = Int32.MaxValue,
                OtherExpenditureFrequency = "test",
                OtherDebts = new List<SaveOtherDebts>(),
                OtherUtilities = Int32.MaxValue,
                OtherUtilitiesArrears = Int32.MaxValue,
                OtherUtilitiesFrequency = "test",
                User = "test",
                UtilitiesTotal = Int32.MaxValue,
                UtilitiesTotalArrears = Int32.MaxValue,
                UtilitiesTotalFrequency = "test",
                Healthcare = Int32.MaxValue,
                HealthcareFrequency = "test",
                HomeContents = Int32.MaxValue,
                HomeContentsArrears = Int32.MaxValue,
                HomeContentsFrequency = "test",
                Housekeeping = Int32.MaxValue,
                HousekeepingFrequency = "test",
                HousingStatus = "test",
                Leisure = Int32.MaxValue,
                LeisureFrequency = "test",
                LowellReference = "test",
                IncomeTotal = Int32.MaxValue,
                OtherIncome = Int32.MaxValue,
                OtherincomeFrequency = "test",
                Pension = Int32.MaxValue,
                PensionFrequency = "test",
                PensionInsurance = Int32.MaxValue,
                PensionInsuranceFrequency = "test",
                PersonalCosts = Int32.MaxValue,
                PersonalCostsFrequency = "test",
                ProfessionalCosts = Int32.MaxValue,
                ProfessionalCostsFrequency = "test",
                Rental = Int32.MaxValue,
                RentArrears = int.MaxValue,
                RentalFrequency = "test",
                Salary = Int32.MaxValue,
                SalaryFrequency = "test",
                SavingsContributions = Int32.MaxValue,
                SavingsContributionsFrequency = "test",
                SchoolCosts = Int32.MaxValue,
                SchoolCostsFrequency = "test",
                SecuredLoans = Int32.MaxValue,
                SecuredloansArrears = Int32.MaxValue,
                SecuredLoansFrequency = "test",
                Travel = Int32.MaxValue,
                TravelFrequency = "test",
                TvLicence = Int32.MaxValue,
                TvLicenceArrears = Int32.MaxValue,
                TvLicenceFrequency = "test"                
            };
        }

        public static OutgoingSourceVm CreateDefaultTestOutgoingSourceVm() 
        {
            return new OutgoingSourceVm()
            {
                Amount = Int32.MaxValue,
                ArrearsAmount = Int32.MaxValue,
                Frequency = "test",
                InArrears = true
            };
        }

        public static BillsAndOutgoingsVm CreateDefaultTestBillsAndOutgoingsVm()
        {
            return new BillsAndOutgoingsVm() 
            {
                ApplianceOrFurnitureRental = CreateDefaultTestOutgoingSourceVm(),
                Ccjs = CreateDefaultTestOutgoingSourceVm(),
                ChildMaintenance = CreateDefaultTestOutgoingSourceVm(),
                Gas = CreateDefaultTestOutgoingSourceVm(),
                Electric = CreateDefaultTestOutgoingSourceVm(),
                TvLicense = CreateDefaultTestOutgoingSourceVm(),
                CouncilTax = CreateDefaultTestOutgoingSourceVm(),
                CourtFines = CreateDefaultTestOutgoingSourceVm(),
                Mortgage = CreateDefaultTestOutgoingSourceVm(),
                Rent = CreateDefaultTestOutgoingSourceVm(),
                OtherFuel = CreateDefaultTestOutgoingSourceVm(),
                Water = CreateDefaultTestOutgoingSourceVm(),
                SecuredLoan = CreateDefaultTestOutgoingSourceVm(),
                IncomeSummary = new MonthlyIncomeVm() 
                {
                    Benefits = 100,
                    Other = 200,
                    Pension = 300,
                    Salary = 400,
                    Total = 1000
                },
                OutgoingSummary = new MonthlyOutgoingsVm() 
                {
                    Expenditures = 1000,
                    HouseholdBills = 2000,
                    Total = 3000
                },
                EnabledPartialSave = true,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "test" } },
                HasErrorPartialSavedIAndE = true,
                PartialSavedEvent = true,
                PartialSavedIAndE = true
            };
        }

        public static ExpenditureSourceVm CreateDefaultTestExpenditureSourceVm() 
        {
            return new ExpenditureSourceVm() 
            {
                Amount = Int32.MaxValue,
                Frequency = "test"
            };
        }

        public static ExpendituresVm CreateDefaultTestExpendituresVm()
        {
            return new ExpendituresVm()
            {
                CareAndHealthCosts = CreateDefaultTestExpenditureSourceVm(),
                CommunicationsAndLeisure = CreateDefaultTestExpenditureSourceVm(),
                FoodAndHouseKeeping = CreateDefaultTestExpenditureSourceVm(),
                PensionsAndInsurance = CreateDefaultTestExpenditureSourceVm(),
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "test" } },
                CommunicationsAndLeisureTriggerMax = Int32.MaxValue,
                CommunicationsAndLeisureTriggerMin = Int32.MaxValue,
                Other = CreateDefaultTestExpenditureSourceVm(),
                EnabledPartialSave = true,
                HasErrorPartialSavedIAndE = true,
                PartialSavedEvent = true,
                PartialSavedIAndE = true,
                Professional = CreateDefaultTestExpenditureSourceVm(),
                Savings = CreateDefaultTestExpenditureSourceVm(),
                PersonalCosts = CreateDefaultTestExpenditureSourceVm(),
                SchoolCosts = CreateDefaultTestExpenditureSourceVm(),
                FoodAndHouseKeepingTriggerMax = Int32.MaxValue,
                FoodAndHouseKeepingTriggerMin = Int32.MaxValue,
                TravelAndTransport = CreateDefaultTestExpenditureSourceVm(),
                PersonalCostsTriggerMax = Int32.MaxValue,
                PersonalCostsTriggerMin = Int32.MaxValue,
                IncomeVmSummary = new MonthlyIncomeVm() 
                {
                    Benefits = Int32.MaxValue,
                    Other = Int32.MaxValue,
                    Pension = Int32.MaxValue,
                    Salary = Int32.MaxValue,
                    Total = Int32.MaxValue
                },
                OutgoingsVmSummary = new MonthlyOutgoingsVm() 
                {
                    Expenditures = Int32.MaxValue,
                    HouseholdBills = Int32.MaxValue,
                    Total = Int32.MaxValue
                }
            };
        }
    }
}
