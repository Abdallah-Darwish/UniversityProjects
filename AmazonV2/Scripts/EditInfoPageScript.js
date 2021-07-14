function setError(element, err) {
    var errorElement = document.createElement("p");
    errorElement.innerHTML = err;
    errorElement.className = "error-par";
    var nextLineBreak = element.nextElementSibling;
    var nnextLineBreak = element.nextElementSibling.nextElementSibling;
    element.parentElement.replaceChild(errorElement, nextLineBreak);
    nnextLineBreak.remove();
}
function clearErrors() {
    var errorsCells = document.querySelectorAll(".error-par");
    for (let i = 0; i < errorsCells.length; i++) {
        errorsCells[i].parentElement.insertBefore(document.createElement("br"), errorsCells[i]);
        errorsCells[i].parentElement.insertBefore(document.createElement("br"), errorsCells[i]);
        errorsCells[i].remove();
    }
}
function validateInput() {
    var txtPassword = document.getElementById("txtPassword");
    var txtConfirmationPassword = document.getElementById("txtConfirmationPassword");
    var txtFirstName = document.getElementById("txtFirstName");
    var txtLastName = document.getElementById("txtLastName");
    var txtAddressLine1 = document.getElementById("txtAddressLine1");
    var txtAddressLine2 = document.getElementById("txtAddressLine2");
    var txtCountry = document.getElementById("txtCountry");
    var txtCity = document.getElementById("txtCity");
    var txtZipCode = document.getElementById("txtZipCode");
    var lstCreditCardType = document.getElementById("lstCreditCardType");
    var txtCreditCardNumber = document.getElementById("txtCreditCardNumber");
    var txtCreditCardName = document.getElementById("txtCreditCardName");
    var dteCreditCardExpiryDate = document.getElementById("dteCreditCardExpiryDate");
    var txtTelephone = document.getElementById("txtTelephone");
    var dteBirthday = document.getElementById("dteBirthday");

    clearErrors();

    // txtPassword.value = txtPassword.value.trim();

    var isValid = true;
    if (isEmpty(txtPassword.value) === false) {
        err = validatePassword(txtPassword.value);
        if (err !== null) {
            setError(txtPassword, err);
            isValid = false;
        } else {
            err = validateRequired(txtConfirmationPassword.value);
            if (err !== null) {
                setError(txtConfirmationPassword, err);
                isValid = false;
            } else if (txtConfirmationPassword.value !== txtPassword.value) {
                setError(txtConfirmationPassword, "Passwords don't match");
                isValid = false;
            }
        }
    }
    err = validateRequired(txtFirstName.value);
    if (err !== null) {
        setError(txtFirstName, err);
        isValid = false;
    }
    err = validateRequired(txtLastName.value);
    if (err !== null) {
        setError(txtLastName, err);
        isValid = false;
    }
    err = validateRequired(txtAddressLine1.value);
    if (err !== null) {
        setError(txtAddressLine1, err);
        isValid = false;
    }
    err = validateRequired(txtAddressLine2.value);
    if (err !== null) {
        setError(txtAddressLine2, err);
        isValid = false;
    }
    err = validateRequired(txtCountry.value);
    if (err !== null) {
        setError(txtCountry, err);
        isValid = false;
    }
    err = validateRequired(txtCity.value);
    if (err !== null) {
        setError(txtCity, err);
        isValid = false;
    }
    err = validateZipCode(txtZipCode.value);
    if (err !== null) {
        setError(txtZipCode, err);
        isValid = false;
    }
    err = validateRequired(txtCreditCardName.value);
    if (err !== null) {
        setError(txtCreditCardName, err);
        isValid = false;
    }
    err = validateCreditCardNumber(txtCreditCardNumber.value);
    if (err !== null) {
        setError(txtCreditCardNumber, err);
        isValid = false;
    }
    err = validateRequired(dteCreditCardExpiryDate.value);
    if (err !== null) {
        setError(dteCreditCardExpiryDate, err);
        isValid = false;
    }
    err = validateTelephone(txtTelephone.value);
    if (err !== null) {
        setError(txtTelephone, err);
        isValid = false;
    }
    err = validateRequired(dteBirthday.value);
    if (err !== null) {
        setError(dteBirthday, err);
        isValid = false;
    }
    return isValid;
}