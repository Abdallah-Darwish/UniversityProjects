<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Repositories\RepositoriesFactory;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$redirect = true;
$order;
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() && array_key_exists("orderId", $_REQUEST)) {
    $order = RepositoriesFactory::getBOrderRepository()->get(intval($_REQUEST["orderId"]));
    if ($order !== null && ($order->customerId === LoginManager::getLoggedInCustomer()->id || LoginManager::isAdmin())) {
        $redirect = false;
    }
}

if ($redirect) {
    header('Location: HomePage.php', true, 303);
    exit();
}
$orderItems = RepositoriesFactory::getBOrderItemRepository()->getByOrder($order->id);
?>
<!DOCTYPE html>

<html>

<head>
    <title>Order <?= $order->id ?></title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/OrderPageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div class="orderId">
        <h2>Order #<?= $order->id ?></h2>
    </div>
    <h2>Details</h2>
    <div id="orderInfoContainer">
        <table class="order-info-table">
            <tr>
                <td class="order-info-name-cell">Order Id: </td>
                <td><?= $order->id ?></td>
            </tr>
            <tr>
                <td class="order-info-name-cell">Order Date: </td>
                <td><?= $order->orderDate->format("Y-m-d") ?></td>
            </tr>
            <tr>
                <td class="order-info-name-cell">Total: </td>
                <td><?= sprintf("$%.2f", $order->total) ?></td>
            </tr>
            <tr>
                <td class="order-info-name-cell">Shipping Address: </td>
                <td><?= $order->shippingAddress ?></td>
            </tr>
        </table>
    </div>
    <hr>
    <h2>Books</h2>
    <div id="orderItemsContainer">
        <table class="order-items-table">
            <tr class="order-items-header">
                <th>Cover</th>
                <th>Name</th>
                <th>Author</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>SubTotal</th>
            </tr>
            <?php
            $booksRepo = RepositoriesFactory::getBookRepository();
            $authorsRepo = RepositoriesFactory::getAuthorRepository();

            foreach ($orderItems as $item) {
                $book = $booksRepo->get($item->bookId);
                $author = $authorsRepo->get($book->authorId);

                echo '<tr class="order-item-row">', "\n";
                echo "<td><a href=\"BookPage.php?bookId=$book->id\">\n";
                echo "<img height=\"150\" width=\"100\" src=\"../Images/book$book->id.jpg\">\n";
                echo '</a></td>', "\n";

                echo "<td><a href=\"BookPage.php?bookId=$book->id\">\n";
                echo "<span class=\"order-item-text\">$book->name</span>\n";
                echo '</a></td>', "\n";

                echo "<td><a href=\"SearchPage.php?name=$author->name\">\n";
                echo "<span class=\"order-item-text\">$author->name</span>\n";
                echo '</a></td>', "\n";

                echo "<td><span class=\"order-item-text\">$item->quantity</span></td>\n";

                echo "<td><span class=\"order-item-text\">$item->unitPrice</span></td>\n";

                echo "<td><span class=\"order-item-text\">", sprintf("%.2f", $item->unitPrice * $item->quantity), "</span></td>\n";
            }
            ?>
        </table>
    </div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>