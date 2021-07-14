<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Repositories\CartItemRepository;
use AmazonV2\Repositories\RepositoriesFactory;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false) {
    header('Location: LoginPage.php', true, 303);
    exit();
}
?>
<!DOCTYPE html>

<html>

<head>
    <title><?= LoginManager::getLoggedInCustomer()->userName ?>'s Wish List</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/CartPageStyle.css" />
    <script type="text/javascript" src="../Scripts/WishListPageScript.js"></script>
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>

    <div class="clear"></div>

    <div id="mainCart">
        <h2>Your Wish List</h2>
        <div id="orderItemsContainer">
            <table class="order-items-table">
                <tr class="order-items-header">
                    <th>Cover</th>
                    <th>Name</th>
                    <th>Author</th>
                </tr>
                <?php
                $booksRepo = RepositoriesFactory::getBookRepository();
                $authorsRepo = RepositoriesFactory::getAuthorRepository();
                $cartItems = RepositoriesFactory::getWishListItemRepository()->getByCustomer(LoginManager::getLoggedInCustomer()->id);
                $numberOfItems = 0;
                foreach ($cartItems as $cartItem) {
                    $book = $booksRepo->get($cartItem->bookId);
                    $author = $authorsRepo->get($book->authorId);
                    echo '<tr class="order-item-row">', "\n";
                    echo "<td class=\"bookId\" style=\"display:none;\">$book->id</td>";
                    echo "<td><a href =\"BookPage.php?bookId=$book->id\">\n";
                    echo "<img height=\"150\" width=\"100\" src=\"../Images/book$book->id.jpg\">\n";
                    echo '</a></td>', "\n";

                    echo "<td><a href=\"BookPage.php?bookId=$book->id\">\n";
                    echo "<span class=\"order-item-text\">$book->name</span>\n";
                    echo '</a></td>', "\n";

                    echo "<td><a href=\"SearchPage.php?name=", urlencode($author->name), "\">\n";
                    echo "<span class=\"order-item-text\">$author->name</span>\n";
                    echo '</a></td>', "\n";
                    echo '<td><button class="remove" onclick="removeItem(this);"> <i class="fas fa-times-circle x"></i> </button></td>', "\n";
                    echo '</tr>', "\n";
                    $numberOfItems += 1;
                }
                ?>
                <tr>
                    <td><br /><br /><br /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr class="lastRows">

                    <td></td>

                    <td><br />Number of items you have in your wish list:<br /><br /> </td>

                    <td></td>

                    <td><?= $numberOfItems ?></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="clear"></div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>