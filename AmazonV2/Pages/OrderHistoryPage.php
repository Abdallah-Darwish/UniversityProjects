<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Repositories\RepositoriesFactory;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$redirect = true;
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false) {
    header('Location: LoginPage.php', true, 303);
    exit();
}

$orders = RepositoriesFactory::getBOrderRepository()->getByCustomer(LoginManager::getLoggedInCustomer()->id);
?>
<!DOCTYPE html>
<html>

<head>
    <title>Your Previous Orders</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/OrderHistoryPageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>

    <div id="history">
        <h2 id="mainTitle">Your order History</h2>

        <?php
        foreach ($orders as $order) {
            echo '<div class="order">', "\n", '<div class="orderDetails">', "\n";
            echo "<p>Order #$order->id</p>\n";
            echo '<span>', $order->orderDate->format('Y-m-d'), "</span><br/>\n";
            echo '<span>', $order->shippingAddress, "</span><br/>\n";
            echo "<p><a href=\"OrderPage.php?orderId=$order->id\">Details...</a></p>\n";
            echo "</div>\n<div class=\"total\">";
            echo sprintf("%.2f", $order->total), "JOD\n";
            echo "</div>\n</div>\n";
            echo "<div class=\"clear\"></div>";
        }
        ?>
        <div class="clear"></div>
        <?php StaticPage::EchoFooter(); ?>
</body>

</html>