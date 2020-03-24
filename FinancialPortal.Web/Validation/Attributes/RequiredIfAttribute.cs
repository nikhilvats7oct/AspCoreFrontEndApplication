using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Web.Validation.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        ////private readonly string _propertyName;

        ////private readonly object _desiredValue;

        ////private readonly RequiredAttribute _innerAttribute;

        ////public RequiredIfAttribute(String propertyName, Object desiredValue,string errorMessage)
        ////{
        ////    _propertyName = propertyName;
        ////    _desiredValue = desiredValue;
        ////    ErrorMessage = errorMessage;
        ////    _innerAttribute =  new RequiredAttribute();
        ////}

        ////public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        ////{
        ////    var rule = new ModelClientValidationRule
        ////    {
        ////        ErrorMessage = ErrorMessageString,
        ////        ValidationType = "requiredif",
        ////    };
        ////    rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(_propertyName);
        ////    rule.ValidationParameters["desiredvalue"] = _desiredValue is bool ? _desiredValue.ToString().ToLower() : _desiredValue;

        ////    yield return rule;
        ////}

        ////protected override ValidationResult IsValid(object value, ValidationContext context)
        ////{
        ////    var dependentProperty = context.ObjectInstance.GetType().GetProperty(_propertyName);

        ////    if (dependentProperty != null)
        ////    {
        ////        var dependentValue = dependentProperty
        ////            .GetValue(context.ObjectInstance, null);

        ////        if (dependentValue.ToString() == _desiredValue.ToString())
        ////        {
        ////            if (!_innerAttribute.IsValid(value))
        ////            {
        ////                return new ValidationResult(ErrorMessage,
        ////                    new[] {context.MemberName});
        ////            }
        ////        }
        ////    }

        ////    return ValidationResult.Success;
        ////}
    }
}
