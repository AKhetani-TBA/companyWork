
function FN_ValidateEmail(field) {
    var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
    return (regex.test(field)) ? true : false;
}

function FN_validateMultipleEmailsCommaSeparated(emailcntl, seperator) {
    var value = emailcntl.value;
    if (value != '') {
        var result = value.split(seperator);
        for (var i = 0; i < result.length; i++) {
            if (result[i] != '') {
                if (!validateEmail(result[i])) {
                    emailcntl.focus();
                    alert('Please check, `' + result[i] + '` email addresses not valid!');
                    return false;
                }
            }
        }
    }
    return true;
}