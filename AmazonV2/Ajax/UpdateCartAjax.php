<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\CartItem;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false) {
    exit();
}
$customerId = LoginManager::getLoggedInCustomer()->id;
$cartItemRepo = RepositoriesFactory::getCartItemRepository();
$bookRepo = RepositoriesFactory::getBookRepository();
$bookId = intval($_REQUEST["bookId"]);
$quantity = intval($_REQUEST["quantity"]);
$cartItem = $cartItemRepo->getByBookId($customerId, $bookId);
if ($quantity <= 0) {
    $cartItemRepo->delete($cartItem->id);
} else if ($cartItem === null) {
    $cartItem = new CartItem();
    $cartItem->bookId = $bookId;
    $cartItem->customerId = $customerId;
    $cartItem->quantity = $quantity;
    $cartItemRepo->create($cartItem);
} else {
    $cartItemRepo->updateQuantity($cartItem->id, $quantity);
}
class CartInfo
{
    public $numberOfItems;
    public $cartTotal;
    public $itemTotal;
}
$info = new CartInfo();
$info->numberOfItems = $cartItemRepo->getCartItemsCount($customerId);
$info->cartTotal = $cartItemRepo->getCartTotal($customerId);
$info->itemTotal = $bookRepo->getPrice($bookId) * max(0, $quantity);
echo json_encode($info);
