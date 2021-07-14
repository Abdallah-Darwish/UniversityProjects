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
if (LoginManager::isLoggedIn() === false) {
    header('Location: LoginPage.php', true, 303);
    exit();
}
$customer = LoginManager::getLoggedInCustomer();
$cartItemRepo = RepositoriesFactory::getCartItemRepository();
$bookRepo = RepositoriesFactory::getBookRepository();
$orderRepo = RepositoriesFactory::getBOrderRepository();
$orderItemRepo = RepositoriesFactory::getBOrderItemRepository();
$cartItems = $cartItemRepo->getCustomerItems($customer->id);

if (count($cartItems) === 0) {
    header('Location: HomePage.php', true, 303);
    exit();
}
foreach ($cartItems as $item) {
    $book = $bookRepo->get($item->bookId);
    if ($book->quantity < $item->quantity) {
        header('Location: CartPage.php?validate=true', true, 303);
        exit();
    }
}
$order = new BOrder();
$order->customerId = $customer->id;
$order->orderDate = new DateTime(\DateTimeZone::ALL);
$order->shippingAddress = "{$customer->addressLine1}, {$customer->addressLine2}, {$customer->city}, {$customer->country}";
$order->total = 0.0;
$orderRepo->create($order);
foreach ($cartItems as $cartItem) {
    $orderItem = new BOrderItem();
    $orderItem->bOrderId = $order->id;
    $orderItem->unitPrice = $bookRepo->getPrice($cartItem->bookId);
    $orderItem->bookId = $cartItem->bookId;
    $orderItem->quantity = $cartItem->quantity;
    $bookRepo->decrementQuantity($cartItem->bookId, $cartItem->quantity);
    $orderItemRepo->create($orderItem);
    $order->total += $orderItem->unitPrice * $orderItem->quantity;
    $cartItemRepo->delete($cartItem->id);
}
$orderRepo->updateTotal($order->id, $order->total);
?>

<!DOCTYPE html>

<html>

<head>
    <title>Our Little Secret</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/CartPageStyle.css" />
    <script type="text/javascript" src="../Scripts/CartPageScript.js"></script>

</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <p>
        We are supposed to send you an email to confirm your order, but we just simply can't because I have to configure a SMTP sever and I really couldn't do that no matter how hard I tried.
        <br>
        <br>
        So instead we are going to pretend that I sent you the email and you agreed!
    </p>
    <?php StaticPage::EchoFooter(); ?>


    <script>
        setTimeout(() => {
            document.location = "OrderPage.php?orderId=<?= $order->id ?>";
        }, 5000);
    </script>
</body>

</html>