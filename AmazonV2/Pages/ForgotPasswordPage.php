<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Repositories\CartItemRepository;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Models\BOrder;
use AmazonV2\Models\BOrderItem;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === true) {
    header('Location: HomePage.php', true, 303);
    exit();
}
if (array_key_exists("userName", $_REQUEST)) {
    $customer = RepositoriesFactory::getCustomerRepository()->getByUserName($_REQUEST["userName"]);
    if ($customer !== null) {
        LoginManager::changePassword($customer->id, "factory_reset");
        echo '<!DOCTYPE html>
       <html>
       
       <head>
           <title>Hello Hacker!</title>';
        StaticPage::EchoSharedLinks();
        echo "</head><body>\n";
        StaticPage::EchoHeader();
        echo '<div class="clear"></div>
        <p>
        Since I have no way to send you an email, I am just going to trust you!!
        <br>
        <br>
        Your new password is "factory_reset"
        </p>
        <script> setTimeout(() => {  document.location = "LoginPage.php";}, 5000);
</script>';
        StaticPage::EchoFooter();
        echo '</body></html>';
        die();
    }
}
?>

<!DOCTYPE html>
<html>

<head>
    <title>Hello Hacker!</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/ForgotPasswordPageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div class="clear"></div>
    <div id="MAIN">
        <div id="main_forgotpassword">
            <div>
                <p>Please Enter your username</p>
                <form method="GET" action="ForgotPasswordPage.php">

                    <input type="text" name="userName" required /> <br /><br />
                    <input type="submit" value="Reset" />

                </form>
            </div>
        </div>
    </div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>