<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\Customer;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$loggedIn = false;

LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn()) {
    $loggedIn = true;
} else if (array_key_exists("userName", $_REQUEST)) {
    $newCustomer = new Customer();

    $newCustomer->firstName = $_REQUEST["firstName"];
    $newCustomer->lastName = $_REQUEST["lastName"];
    $newCustomer->telephone = $_REQUEST["telephone"];
    $newCustomer->email = $_REQUEST["email"];
    $newCustomer->addressLine1 = $_REQUEST["addressLine1"];
    $newCustomer->addressLine2 = $_REQUEST["addressLine2"];
    $newCustomer->country = $_REQUEST["country"];
    $newCustomer->city = $_REQUEST["city"];
    $newCustomer->zipCode = $_REQUEST["zipCode"];
    $newCustomer->creditCardType = $_REQUEST["creditCardType"];
    $newCustomer->creditCardNumber = $_REQUEST["creditCardNumber"];
    $newCustomer->creditCardExpiryDate = new DateTime($_REQUEST["creditCardExpiryDate"]);
    $newCustomer->creditCardName = $_REQUEST["creditCardName"];
    $newCustomer->gender = $_REQUEST["gender"];
    $newCustomer->birthday = new DateTime($_REQUEST["birthday"]);
    $newCustomer->passwordHash = $_REQUEST["password"];
    $newCustomer->userName = $_REQUEST["userName"];
    $newCustomer->registrationDate = new DateTime();
    LoginManager::Register($newCustomer);
    LoginManager::Login($newCustomer->userName, $_REQUEST["password"], false);
    $loggedIn = true;
}

if ($loggedIn) {
    header('Location: HomePage.php', true, 303);
    exit();
}
?>
<!DOCTYPE html>
<html>

<head>
    <title>Register</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/LoginPageStyle.css" />
    <script type="text/javascript" src="../Scripts/CustomerInfoValidators.js"></script>
    <script type="text/javascript" src="../Scripts/RegisterPageScript.js"></script>
    <script type="text/javascript" src="../Scripts/WishListPageScript.js"></script>
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div id="banner">
        <img src="../Images/join.png" width="300" height="200" />
    </div>
    <form action="RegisterPage.php" method="GET" onsubmit="return validateInput();">
        <div class="container">
            <table class="input-table">
                <tr>
                    <td class="wide-label-cell">
                        <label for="txtEmail"><b>Email</b></label>
                    </td>
                    <td>
                        <input id="txtEmail" type="text" placeholder="email@mail.com" name="email">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtUserName"><b>Username</b></label>
                    </td>
                    <td>
                        <input id="txtUserName" type="text" placeholder="Enter Username" name="userName">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <br />
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtPassword"><b>Password</b></label>
                    </td>
                    <td>
                        <input id="txtPassword" type="password" placeholder="Enter Password" name="password">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtConfirmationPassword"><b>Confirm Password</b></label>
                    </td>
                    <td>
                        <input id="txtConfirmationPassword" type="password" placeholder="Enter Password Again!">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtFirstName"><b>First name</b></label>
                    </td>
                    <td>
                        <input id="txtFirstName" type="text" placeholder="Enter First Name" name="firstName">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtLastName"><b>Last name</b></label>
                    </td>
                    <td>
                        <input id="txtLastName" type="text" placeholder="Enter Last Name" name="lastName">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtAddressLine1"><b>Address Line 1</b></label>
                    </td>
                    <td>
                        <input id="txtAddressLine1" type="text" placeholder="Enter Address Line 1" name="addressLine1">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtAddressLine2"><b>Address Line 2</b></label>
                    </td>
                    <td>
                        <input id="txtAddressLine2" type="text" placeholder="Enter Address Line 2" name="addressLine2">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtCountry"><b>Country</b></label>
                    </td>
                    <td>
                        <input id="txtCountry" type="text" placeholder="Enter Country" name="country">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtCity"><b>City</b></label>
                    </td>
                    <td>
                        <input id="txtCity" type="text" placeholder="Enter City" name="city">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtZipCode"><b>Zip Code</b></label>
                    </td>
                    <td>
                        <input id="txtZipCode" type="text" placeholder="Enter Zip Code" name="zipCode">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="lstCreditCardType"><b>Credit Card Type</b></label>
                    </td>
                    <td>
                        <select id="lstCreditCardType" name="creditCardType">
                            <option value="Visa" selected>Visa</option>
                            <option value="MasterCard">Master Card</option>
                            <option value="AmericanExpress">American Express</option>
                        </select>
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtCreditCardNumber"><b>Credit Card Number</b></label>
                    </td>
                    <td>
                        <input id="txtCreditCardNumber" type="text" placeholder="Enter Credit Card Number" name="creditCardNumber">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtCreditCardName"><b>Credit Card Name</b></label>
                    </td>
                    <td>
                        <input id="txtCreditCardName" type="text" placeholder="Enter Name On The Card" name="creditCardName">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="dteCreditCardExpiryDate"><b>Credit Card Expiry Date</b></label>
                    </td>
                    <td>
                        <input id="dteCreditCardExpiryDate" type="date" name="creditCardExpiryDate">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="txtTelephone"><b>Telephone number</b></label>
                    </td>
                    <td>
                        <input id="txtTelephone" type="text" placeholder="Enter Telephone Number" name="telephone">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="dteBirthday"><b>Birthday</b></label>
                    </td>
                    <td>
                        <input id="dteBirthday" type="date" name="birthday" max="<?= date("Y-m-d") ?>">
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell">
                        <label for="lstGender"><b>Gender</b></label>
                    </td>
                    <td>
                        <select id="lstGender" name="gender">
                            <option value="male" selected>Male</option>
                            <option value="female">Female</option>
                        </select>
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="wide-label-cell"></td>
                    <td>
                        <button type="submit" id="btnRegister">Register</button>
                    </td>
                    <td class="error-cell">
                        <label>
                        </label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear"></div>
        <div class="container" style="background-color:#f1f1f1">
            <span class="forgot"><a href="ForgotPasswordPage.php">Forgot password?</a></span>
            <span id="register"><a href="RegisterPage.php">Don't have an account?</a></span>
        </div>
    </form>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>