$.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'targetvalue'], function (options) {
    options.rules['requiredif'] = options.params;
    options.messages['requiredif'] = options.message;
});

$.validator.addMethod('requiredif', function (value, element, parameters) {
    console.log("dependent property -" + parameters.dependentproperty);
    var targetvalue = parameters.targetvalue;
    targetvalue = (targetvalue == null ? '' : targetvalue).toString();
    var controlType = $("input[name='" + parameters.dependentproperty + "']").attr("type");

    if (controlType == undefined) {
        controlType = $("input[name$='" + parameters.dependentproperty + "']").attr("type");
    }
    var actualvalue = {};
    console.log("control type -" + controlType);

    if (controlType === "checkbox") {
        var control = $("input[name$='" + parameters.dependentproperty + "']:checked");
        actualvalue = control.val();
    }
    else if (controlType === "radio") {
        var radiocontrol = $("input[type=radio][name=" + parameters.dependentproperty + "]:checked");
        actualvalue = radiocontrol.val();
    }
    else {
        actualvalue = $("#" + parameters.dependentproperty).val();
    }

    console.log(targetvalue);
    console.log(actualvalue);

    if ($.trim(targetvalue).toLowerCase() === $.trim(actualvalue).toLocaleLowerCase()) {
        console.log("debugging");
        var isValid = $.validator.methods.required.call(this, value, element, parameters);
        return isValid;
    }
    return true;
});