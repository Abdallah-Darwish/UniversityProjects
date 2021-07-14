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
$validate = boolval($_REQUEST["validate"]);
?>
<!DOCTYPE html>

<html>

<head>
    <title><?= LoginManager::getLoggedInCustomer()->userName ?>'s Cart</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/CartPageStyle.css" />
    <script type="text/javascript" src="../Scripts/CartPageScript.js"></script>

</head>

<body onload="setup();">
    <?php StaticPage::EchoHeader(); ?>

    <div class="clear"></div>

    <div id="mainCart">
        <h2>Your cart details</h2>
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
                $cartItems = RepositoriesFactory::getCartItemRepository()->getCustomerItems(LoginManager::getLoggedInCustomer()->id);
                $total = 0.0;
                $numberOfItems = 0;
                foreach ($cartItems as $cartItem) {
                    $book = $booksRepo->get($cartItem->bookId);
                    $author = $authorsRepo->get($book->authorId);
                    echo '<tr class="order-item-row">', "\n";
                    echo "<td class=\"bookId\" style=\"display:none;\">$book->id</td>";
                    echo "<td><a href=\"BookPage.php?bookId=$book->id\">\n";
                    echo "<img height=\"150\" width=\"100\" src=\"../Images/book$book->id.jpg\">\n";
                    echo '</a></td>', "\n";

                    echo "<td><a href=\"BookPage.php?bookId=$book->id\">\n";
                    echo "<span class=\"order-item-text\">$book->name</span>\n";
                    echo '</a></td>', "\n";

                    echo "<td><a href=\"SearchPage.php?name=", urlencode($author->name), "\">\n";
                    echo "<span class=\"order-item-text\">$author->name</span>\n";
                    echo '</a></td>', "\n";

                    echo "<td><input type=\"number\" value=\"$cartItem->quantity\" min=\"1\" oninput=\"updateItem(this);\"/></td>\n";

                    $unitPrice = $booksRepo->getPrice($book->id);
                    echo "<td><span class=\"order-item-text\">$unitPrice</span></td>\n";

                    $subTotal = $unitPrice * $cartItem->quantity;
                    echo "<td><span class=\"cellSubTotal\">", sprintf("%.2f", $subTotal), "</span></td>\n";

                    echo '<td><button class="remove" onclick="removeItem(this);"> <i class="fas fa-times-circle x"></i> </button></td>', "\n";
                    if ($validate) {
                        if ($book->quantity === 0) {
                            echo "<td class=\"error-cell\"><label>This book is currently out of stock</label></td>\n";
                        } else if ($book->quantity < $cartItem->quantity) {
                            echo "<td class=\"error-cell\"><label>Maximum available quantity is $book->quantity</label></td>\n";
                        }
                    }
                    echo '</tr>', "\n";
                    $total += $subTotal;
                    $numberOfItems += $cartItem->quantity;
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

                    <td><br /><label>Number of items you have in your cart:</label><br /><br /> </td>

                    <td></td>

                    <td id="cellNumberOfItems"><label><?= $numberOfItems ?></label></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr class="lastRows">

                    <td></td>

                    <td><br />Total price:<br /><br /> </td>

                    <td></td>

                    <td id="cellTotalPrice"><?= sprintf("%.2f JOD", $total) ?></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <div class="clear"></div>
            <div id="checkOutDiv">
                <img src="../Images/cart.png" class="books" />
                <button id="checkout" onclick="ccheckout();">Checkout</button>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
        <?php StaticPage::EchoFooter(); ?>
    </div>


</body>

</html>