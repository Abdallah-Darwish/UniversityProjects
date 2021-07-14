<?php
use AmazonV2\Models\Book;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\BookRating;
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\BookRepository;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Repositories\BookRatingRepository;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

$invalidBook = true;
$book = null;
$discount = null;
$author = null;
$bookRepo = RepositoriesFactory::getBookRepository();
LoginManager::loginUsingCookie();

if (array_key_exists("bookId", $_REQUEST)) {
    $book = $bookRepo->get(intval($_REQUEST["bookId"]));
    if ($book !== null) {
        $invalidBook = false;
        $discount = RepositoriesFactory::getDiscountRepository()->getFirstActive($book->id, new DateTime());
        $author = RepositoriesFactory::getAuthorRepository()->get($book->authorId);
    }
}

if ($invalidBook) {
    header('Location: HomePage.php', true, 303);
    exit();
}
$bookRepo->incrementViews($book->id);
?>
<!DOCTYPE html>
<html>

<head>
    <title><?= $book->name ?></title>
    <link rel="stylesheet" type="text/css" href="../Styles/BookPageStyle.css" />
    <?php StaticPage::EchoSharedLinks() ?>
    <script type="text/javascript" src="../Scripts/BookPageScript.js"></script>
    <style>
        footer {
            margin-top: 50px;
        }
    </style>
</head>

<body onload="setup();">
    <?php StaticPage::EchoHeader(); ?>
    <input id="bookId" type="hidden" value="<?= $book->id ?>" />


    <div id="itemMain">

        <div id="ad">
            <br />
            <br />
            AN AD
            <br />
            <br />
            <br />
        </div>

        <div id="theItem">
            <img src="../Images/book<?= $book->id ?>.jpg" />
            <h2><?= $book->name ?></h2>

            <div id="specifications">
                <?php
                $roundedRating = (int)round($book->rating);
                for ($i = 1; $i <= $roundedRating; $i++) {
                    echo '<i class="fas fa-star stars2"></i>';
                }
                for ($i = $roundedRating + 1; $i <= 5; $i++) {
                    echo '<i class="fas fa-star stars1"></i>';
                }
                ?>
                <span><?= sprintf("%.2f(%d  ratings by Goodreads)", $book->rating, $book->ratersCount) ?></span>
                <p> <?= $book->description ?></p>
            </div>

        </div>

        <div id="price-buy">
            <p id="pricePar">
                <?php
                if ($discount === null) {
                    echo $book->unitPrice, "JOD";
                } else {
                    echo '<span style="text-decoration: line-through;color:salmon;">', $book->unitPrice, 'JOD</span></br>', " ", $discount->unitPrice, "JOD";
                }
                ?>

            </p>
            <?php if ($discount === null) {
                echo '<br>';
            } ?>

            <h1 style="color:<?= ($book->quantity > 0 ? 'green' : 'salmon') ?>;"><?= ($book->quantity > 0 ? 'In Stock' : 'Out Of Stock') ?></h1>

            <br />
            <?php
            if (LoginManager::isLoggedIn()) {
                $cartItemRepo = RepositoriesFactory::getCartItemRepository();
                echo '<button id="btnAddToCart" class="add-to-cart" onclick="addToCart();"' . ($cartItemRepo->getByBookId(LoginManager::getLoggedInCustomer()->id, $book->id) !== null ? "hidden" : "") . '>Add to Cart</button>';
                echo '<button id="btnRemoveFromCart" class="add-to-cart" onclick="removeFromCart();"' . ($cartItemRepo->getByBookId(LoginManager::getLoggedInCustomer()->id, $book->id) === null ? "hidden" : "") . '>Remove From Cart</button>';
                echo '<br/>';
                $wishListItemRepo = RepositoriesFactory::getWishListItemRepository();
                echo '<button id="btnAddToWishList" class="add-to-wish-list" onclick="addToWishList();"' . ($wishListItemRepo->isInCustomerWishList(LoginManager::getLoggedInCustomer()->id, $book->id) ? "hidden" : "") . '>Add to Wish List</button>';
                echo '<button id="btnRemoveFromWishList" class="add-to-wish-list" onclick="removeFromWishList();"' . ($wishListItemRepo->isInCustomerWishList(LoginManager::getLoggedInCustomer()->id, $book->id) ? "" : "hidden") . '>Remove From Wish List</button>';
            } else {
                echo '<button id="btnAddToCart" class="add-to-cart" onclick="window.open(\'LoginPage.php\')">Add to Cart</button>';
                echo '<br/>';
                echo '<button id="btnAddToWishList" class="add-to-wish-list" onclick="window.open(\'LoginPage.php\')">Add to Wish List</button>';
            }
            ?>
        </div>

        <div class="clear"></div>

        <div id="productDesc">
            <h5>Product's description</h5>
            <table cellpadding="10">
                <tr>
                    <td><b>For ages:</b> <?= "$book->minAge - $book->maxAge" ?></td>
                    <td><b>Publication city/Country:</b> <?= $book->publicationLocation ?></td>
                </tr>
                <tr>
                    <td><b>Format:</b> <?= "$book->coverType | $book->numberOfPages" ?></td>
                    <td><b>Publication Date:</b><?= $book->publicationDate->format("Y-m-d") ?></td>
                </tr>
                <tr>
                    <td><b>Publisher:</b> <?= $book->publisher ?></td>
                </tr>
            </table>
        </div>

        <div id="aboutWriter">
            <h5>About <?= $author->name ?></h5>
            <p><?= $author->bio ?></p>
        </div>
        <div id="categoriesDiv">
            <h5>Categories</h5>
            <p>
                <?php
                $sp = false;
                $categories = RepositoriesFactory::getCategoryRepository()->getByBook($book->id, 10);
                foreach ($categories as $category) {
                    if ($sp) {
                        echo ', ';
                    }
                    echo "<a href=\"SearchPage.php?category[]=$category->id\">$category->name</a>";
                    $sp = true;
                }
                ?>
            </p>
        </div>
        <div id="ratingDiv">
            <h5>Rating Details</h5>
            <div id="meters">
                <span>5 <i class="fas fa-star stars2"></i> <meter value="<?= sprintf("%.2f", $book->rating5) ?>"></meter> <?= (int)($book->rating5 * 100.0) ?>%</span>

                <br /><br />

                <span>4 <i class="fas fa-star stars2"></i><meter value="<?= sprintf("%.2f", $book->rating4) ?>"></meter> <?= (int)($book->rating4 * 100.0) ?>%</span>

                <br /><br />

                <span>3 <i class="fas fa-star stars2"></i><meter value="<?= sprintf("%.2f", $book->rating3)  ?>"></meter> <?= (int)($book->rating3 * 100.0) ?>%</span>

                <br /><br />

                <span>2 <i class="fas fa-star stars2"></i><meter value="<?= sprintf("%.2f", $book->rating2) ?>"></meter> <?= (int)($book->rating2 * 100.0) ?>%</span>

                <br /><br />

                <span>1 <i class="fas fa-star stars2"></i><meter value="<?= sprintf("%.2f", $book->rating1)  ?>"></meter> <?= (int)($book->rating1 * 100.0) ?>%</span>

                <br /><br />
            </div>
        </div>
        <div class="clear"></div>
        <?php StaticPage::EchoFooter(); ?>
    </div>
    <div class="clear"></div>

</body>

</html>