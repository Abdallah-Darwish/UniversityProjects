<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$invalid_userName = false;
$invalid_password = false;
$loggedIn = false;
$userName = "";
if (array_key_exists("userName", $_REQUEST)) {
    $userName = $_REQUEST["userName"];
}
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn()) {
    $loggedIn = true;
} else if (strlen($userName) > 0) {
    $password = $_REQUEST["password"];
    $remember = array_key_exists("remember", $_REQUEST);
    try {
        LoginManager::Login($userName, $password, $remember);
    } catch (Exception $ex) {
        if (strpos($ex->getMessage(), "name") !==  false) {
            $invalid_userName = true;
        } else {
            $invalid_password = true;
        }
    }
    $loggedIn = $invalid_userName === false && $invalid_password === false;
}

if ($loggedIn) {
    header('Location: HomePage.php', true, 303);
    exit();
}
?>
<!DOCTYPE html>

<html>

<head>
    <title>Login</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/LoginPageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div id="banner">
        <img src="../Images/login.jpg" width="300" height="200" />
    </div>

    <form id="loginForm" action="LoginPage.php" method="GET">
        <div class="container">
            <table class="input-table" border="0">
                <tr>
                    <td class="label-cell">
                        <label for="txtUserName"><b>Username</b></label>
                    </td>
                    <td>
                        <input id="txtUserName" type="text" placeholder="Enter Username" name="userName" required value="<?= $userName ?>">
                    </td>
                    <td class="error-cell">
                        <label>
                            <?= $invalid_userName ? 'Username doesn\'t exist!' : '' ?>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">
                        <label for="txtPassword"><b>Password</b></label>
                    </td>
                    <td>
                        <input id="txtPassword" type="password" placeholder="Enter Password" name="password" required>
                    </td>
                    <td class="error-cell">
                        <label>
                            <?= $invalid_password ? 'Incorrect password!' : '' ?>
                        </label>

                    </td>
                </tr>
                <tr>
                    <td class="label-cell"></td>
                    <td>
                        <button type="submit" id="btnLogin">Login</button>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell"></td>
                    <td>
                        <input id="chkRememberMe" type="checkbox" checked="checked" name="remember"> <label for="remember">Remember me</label>
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