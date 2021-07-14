<?php
use AmazonV2\Pages\StaticPage;
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\RepositoriesFactory;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$redirect = true;
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false) {
    header('Location: LoginPage.php', true, 303);
    exit();
}

$customer = LoginManager::getLoggedInCustomer();
if (array_key_exists("firstName", $_REQUEST)) {
    $customer->firstName = $_REQUEST["firstName"];
    $customer->lastName = $_REQUEST["lastName"];
    $customer->telephone = $_REQUEST["telephone"];
    $customer->addressLine1 = $_REQUEST["addressLine1"];
    $customer->addressLine2 = $_REQUEST["addressLine2"];
    $customer->country = $_REQUEST["country"];
    $customer->city = $_REQUEST["city"];
    $customer->zipCode = $_REQUEST["zipCode"];
    $customer->creditCardType = $_REQUEST["creditCardType"];
    $customer->creditCardNumber = $_REQUEST["creditCardNumber"];
    $customer->creditCardExpiryDate = new DateTime($_REQUEST["creditCardExpiryDate"]);
    $customer->creditCardName = $_REQUEST["creditCardName"];
    $customer->gender = $_REQUEST["gender"];
    $customer->birthday = new DateTime($_REQUEST["birthday"]);

    $customerRepo = RepositoriesFactory::getCustomerRepository();
    $customerRepo->update($customer);

    if (strlen(trim($_REQUEST["password"])) > 0) {
        LoginManager::changePassword($customer->id, trim($_REQUEST["password"]));
    }
}
$customer = LoginManager::getLoggedInCustomer();
?>
<!DOCTYPE html>

<html>

<head>
    <title>Your Profile Settings</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/EditInfoPageStyle.css" />
    <script type="text/javascript" src="../Scripts/CustomerInfoValidators.js"></script>
    <script type="text/javascript" src="../Scripts/EditInfoPageScript.js"></script>
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div id="editHeader">
        <h2>Edit your profile settings</h2>
    </div>
    <form style="margin: 0 auto;width: 1300px;" onsubmit="return validateInput();">
        <div id="userInfo">
            <h3><i class="far fa-address-card"></i></h3>
            <input type="text" name="firstName" id="txtFirstName" placeholder="    First Name" value="<?= $customer->firstName ?>" />
            <br /><br />
            <input type="text" name="lastName" id="txtLastName" placeholder="    Last Name" value="<?= $customer->lastName ?>" />
            <br /><br />
            <input type="password" name="password" id="txtPassword" placeholder="    Password" />
            <br /><br />
            <input type="password" id="txtConfirmationPassword" placeholder="    Confirmation Password" />
            <br /><br />
            <input type="text" name="telephone" id="txtTelephone" placeholder="    Phone Number" value="<?= $customer->telephone ?>" />
            <br /><br />
        </div>
        <div id="billingAddress">
            <h3>Your billing Info</h3>
            <input type="text" name="addressLine1" id="txtAddressLine1" placeholder="    Address Line 1" value="<?= $customer->addressLine1 ?>" />
            <br /><br />
            <input type="text" name="addressLine2" id="txtAddressLine2" placeholder="    Address Line 2" value="<?= $customer->addressLine2 ?>" />
            <br /><br />
            <input type="text" name="city" id="txtCity" placeholder="    City" value="<?= $customer->city ?>" />
            <br /><br />
            <input type="text" name="country" id="txtCountry" placeholder="    Country" value="<?= $customer->country ?>" />
            <br /><br />
            <input type="text" name="zipCode" id="txtZipCode" placeholder="    Zip Code" value="<?= $customer->zipCode ?>" />
            <br /><br />
        </div>
        <div id="ccInfo">
            <h3>Your Credit Card Info</h3>
            <span>The type of your Credit Card:</span> <br /><br />
            <input type="radio" name="creditCardType" value="Visa" <?= $customer->creditCardType === "Visa" ? "checked" : "" ?> />Visa <br />
            <input type="radio" name="creditCardType" value="MasterCard" <?= $customer->creditCardType === "MasterCard" ? "checked" : "" ?> />MasterCard <br />
            <input type="radio" name="creditCardType" value="AmericanExpress" <?= $customer->creditCardType === "AmericanExpress" ? "checked" : "" ?> />American Express <br /><br />

            <input type="text" name="creditCardNumber" id="txtCreditCardNumber" placeholder="    Credit Card Number" value="<?= $customer->creditCardNumber ?>" />
            <br /><br />
            <input type="date" name="creditCardExpiryDate" id="dteCreditCardExpiryDate" value="<?= $customer->creditCardExpiryDate->format('Y-m-d') ?>" />
            <br /><br />
            <input type="text" name="creditCardName" id="txtCreditCardName" placeholder="    Name on Card" value="<?= $customer->creditCardName ?>" />
            <br /><br />
        </div>
        <div id="info">
            <h3>
                <i class="far fa-address-card"></i>
            </h3>
            <span>Gender:</span> <br />
            <input type="radio" name="gender" value="female" <?= $customer->gender === "female" ? "checked" : "" ?> /> Female <br />
            <input type="radio" name="gender" value="male" <?= $customer->gender === "male" ? "checked" : "" ?> /> Male<br />
            <br />
            <label for="dteBirthday">Your Birthday: </label><input type="date" name="birthday" id="dteBirthday" style="width:120px;" value="<?= $customer->birthday->format("Y-m-d") ?>" />
            <br /><br />
        </div>
        <div class="clear"></div>
        <button>Submit Changes</button>
    </form>
    <div class="clear"></div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>