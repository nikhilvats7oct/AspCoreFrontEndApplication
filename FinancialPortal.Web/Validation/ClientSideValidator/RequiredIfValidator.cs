using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace FinancialPortal.Web.Validation.ClientSideValidator
{
    public class RequiredIfValidator : PropertyValidator
    {
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public RequiredIfValidator(string dependentProperty, object targetValue,string errorMessage) : base(errorMessage)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            //This is not a server side validation rule. So, should not effect at the server side
            return true;
        }
    }
}
