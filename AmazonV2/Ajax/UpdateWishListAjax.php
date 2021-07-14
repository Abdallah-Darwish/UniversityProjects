<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\WishListItem;
use AmazonV2\Repositories\WishListItemRepository;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();
LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false) {
    exit();
}

$wishListItemRepo = RepositoriesFactory::getWishListItemRepository();
if (array_key_exists("add", $_REQUEST)) {
    $bookId =  intval($_REQUEST["add"]);
    if ($wishListItemRepo->isInCustomerWishList(LoginManager::getLoggedInCustomer()->id, $bookId) === false) {
        $wishListItem = new WishListItem();
        $wishListItem->customerId = LoginManager::getLoggedInCustomer()->id;
        $wishListItem->bookId = $bookId;
        $wishListItemRepo->create($wishListItem);
    }
} else if (array_key_exists("remove", $_REQUEST)) {
    $wishListItemRepo->deleteFromCustomerWishList(LoginManager::getLoggedInCustomer()->id, intval($_REQUEST["remove"]));
}
