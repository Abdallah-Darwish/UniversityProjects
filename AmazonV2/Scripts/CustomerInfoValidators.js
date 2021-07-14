var req = "This field is required!";
function isEmpty(val) {
    return val.trim().length === 0;
}
function validatePassword(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^[^\s]{8,}$/) !== 0) { err = "Password must be at least 8 characters and can't contain spaces"; }
    return err;
}
function validateEmail(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/) !== 0) { err = "Invalid email address"; }
    callServer("../Ajax/RegisterPageAjax.php", "email=" + encodeURIComponent(txtEmail.value.trim()), (txt) => {
        if (txt === "false") {
            err = "This email is already registered";
        }
    }, false);
    return err;
}
function validateUserName(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^[a-zA-Z0-9_.+-@]{8,16}$/) !== 0) { err = "Username must be between 8 to 16 characters, numbers and _ . + - @"; }
    else {
        callServer("../Ajax/RegisterPageAjax.php", "userName=" + encodeURIComponent(txtUserName.value.trim()), (txt) => {
            if (txt === "false") {
                err = "This username is already taken";
            }
        }, false);
    }
    return err;
}
function validateRequired(val) {
    if (isEmpty(val)) { return req; }
    return null;
}
function validateZipCode(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^\d{5}$/) !== 0) { err = "Zip code must consist of 5 numbers only"; }
    return err;
}
function validateCreditCardNumber(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^\d{4}-\d{4}-\d{4}-\d{4}$/) !== 0) { err = "Credit card number must be in-format\"####-####-####-####\""; }
    return err;
}
function validateTelephone(val) {
    var err = null;
    if (isEmpty(val)) { err = req; }
    else if (val.search(/^(\+|(00))\d{1,4}\d{9}$/) !== 0) { err = "Telephone number must be in format 00#[###]#########"; }
    return err;
}