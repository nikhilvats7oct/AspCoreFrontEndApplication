
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
    parent.postMessage(JSON.stringify({ 'docHeight': ht, 'messageType': 'ResizeCallbackIFrame' }), '*');
    console.log("message has sent to parent");
}

//send docHeight on button click
$('#btnCallback').on('click',
    function () {
        $('.clsbtn-callback').addClass('d-none');
        $('.clsform-callback').removeClass('d-none');

        var ht = 1125;
        console.log("Document height :" + ht);
        parent.postMessage(JSON.stringify({ 'docHeight': ht, 'messageType': 'ResizeCallbackIFrame' }), '*');
        console.log("message has sent to parent");
    });

$(document).ready(function () {
    $.validator.addMethod(
        "date",
        function (value, element) {
            var bits = value.match(/([0-9]+)/gi), str;
            if (!bits)
                return this.optional(element) || false;
            str = bits[1] + '/' + bits[0] + '/' + bits[2];
            return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
        },
        "Please enter a date in the format dd/mm/yyyy"
    );

    $('.js-datepicker-callback').first().change(function (e) {
        $(this).attr('value', $(this).val());
    });

    $(".Appointment").prop("checked", true);
    $('.clscallbackdate').removeClass('d-none');
    $('.clscallbacktime').removeClass('d-none');
});
