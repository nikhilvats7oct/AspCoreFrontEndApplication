// This is to address iPhone issue where button click event are not firing. 
document.addEventListener('touchstart', {}); 

//adding event listener for windows
if (window.addEventListener) {
    window.addEventListener('load', sendDocHeightMsg, false);
} else if (window.attachEvent) { // ie8
    window.attachEvent('onload', sendDocHeightMsg);
}

function getDocHeight(doc) {
    doc = doc || document;

    //get the height from the div wrapper
    var contentDiv = $("div:first");
    if (contentDiv.length > 0) {
        return contentDiv.height();
    } else {
        var body = doc.body, html = doc.documentElement;
        var height = Math.max(body.scrollHeight, body.offsetHeight,
            html.clientHeight, html.scrollHeight, html.offsetHeight);
        return height;
    }
}

// send docHeight onload
function sendDocHeightMsg(e) {
    var ht = getDocHeight();
    console.log("Document height :" + ht);
    parent.postMessage(JSON.stringify({ 'docHeight': ht, 'messageType': 'ResizeContactUsIFrame' }), '*');
    console.log("message has sent to parent");
}

$(document).ready(function () {
    $.validator.addMethod(
        "date",
        function (value, element) {
            var parsedDate = window.moment(value, "DD-MMMM-YYYY");
            var result = this.optional(element) || parsedDate.isValid();

            return result;
        },
        "Please enter a date in the format dd/MMMM/yyyy"
    );

    $('.date-of-birth-text').on('change',
        function (e) {
            $(this).attr('value', $(this).val());
        });

    var passworddiv = $('[name=divthridpartypassword]');

    $('input[type=radio][name=AccountHolderStatus]').change(function () {

        var authorisedoptions = "Authorised 3rd Party";

        if (this.value === authorisedoptions) {
            passworddiv.show();

        } else {
            passworddiv.hide();
        }
    });

    $('.btncontactus').bind('click', function () {
        $('.btncontactus').addClass('d-none');
        $('.contactusform').removeClass('d-none');
    });
});
