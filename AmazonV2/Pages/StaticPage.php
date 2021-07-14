<?php
namespace AmazonV2\Pages;

require_once(dirname(__FILE__, 2) . '\UserSystem\LoginManager.php');

use AmazonV2\UserSystem;
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Models\Book;
use AmazonV2\Repositories\CartItemRepository;
use DateTime;

error_reporting(0);
class StaticPage
{
    public static function echoBookDiv(Book $book): void
    {
        $bookPrice = $book->unitPrice;
        $author = RepositoriesFactory::getAuthorRepository()->get($book->authorId);
        echo '<div class="itemDiv">', "\n";
        echo "<a href=\"BookPage.php?bookId=$book->id\"><img src=\"../Images/book$book->id.jpg\"/></a>\n";
        $discount = RepositoriesFactory::getDiscountRepository()->getFirstActive($book->id);
        if ($discount !== null) {
            $bookPrice = $discount->unitPrice;
            $discountPercentage = (int)(($book->unitPrice - $discount->unitPrice) / $book->unitPrice * 100);
            echo "<div class=\"saleTag\">$discountPercentage% off</div>\n";
        }
        echo "<p class=\"bName\">$book->name</p>\n";
        echo "<p class=\"writer\">$author->name</p>\n";
        $roundedRating = (int)round($book->rating);
        echo '<p class="item-div-rating">', "\n";
        for ($i = 1; $i <= $roundedRating; $i++) {
            echo "<i class=\"fas fa-star stars3 yellowStar\"></i>\n";
        }
        for ($i = $roundedRating + 1; $i <= 5; $i++) {
            echo "<i class=\"fas fa-star stars3\"></i>\n";
        }
        echo "</p>\n";
        echo "<p class=\"price\">$bookPrice JOD</p>\n";
        echo "</div>\n";
    }
    public static function EchoHeader(): void
    {
        echo '<div id="header" style="position:relative;">
        <a href="Homepage.php"><img src="../Images/Logo2.png" alt="Logo" /></a>
        <div id="cart">
            <a href="CartPage.php"><span id="spnCartTotal">';
        if (LoginManager::isLoggedIn()) {
            $cartItemRepo = RepositoriesFactory::getCartItemRepository();
            echo $cartItemRepo->getCartTotal(LoginManager::getLoggedInCustomer()->id), ' JOD</span><i class="fas fa-shopping-cart"></i></a>
            </div>

            <nav id="profileDropDown" class="drp" role="navigation">
            <ul>
              <li><a> <i class="far fa-user"></i> Profile</a>
                <ul class="dropdown">
                  <li><a href="EditInfoPage.php">Edit profile</a></li>
                  <li><a href="WishListPage.php">Wish list</a></li>
                  <li><a href="OrderHistoryPage.php">Order history</a></li>';
            if (LoginManager::isAdmin()) {
                echo '<li><a href="AdminDiscountPage.php">Create Discount</a></li>';
            }
            echo '<li><a href="Logout.php">Logout</a></li>
                </ul>
              </li>
            </ul>
          </nav>';
        } else {
            echo '0 JOD</span><i class="fas fa-shopping-cart"></i></a>
                    </div>
                    <label id="lblLogin">
                    <a href="LoginPage.php" target="_top">
                    <i class="far fa-user header-content"></i>
                    <span class="header-content">Login</span></a>
                    </label>
                </div>';
        }
        echo '</div>';
    }
    public static function EchoFooter(): void
    {
        echo '<footer>
        <div id="contactUs">
            <span>Contact Us</span> <br /><br />
            <a href="https://twitter.com/twitterbooks?lang=en" target="_blank"><i class="fab fa-twitter"></i></a>
            <a href="https://www.facebook.com/BuzzFeedBooks/" target="_blank"> <i class="fab fa-facebook-square"></i></a>
            <a href="https://www.instagram.com/explore/tags/book/?hl=en" target="_blank"><i
                    class="fab fa-instagram"></i></a>
        </div>
        <i class="far fa-copyright" id="copyfont"></i>
        <span id="Copyrights">2019 All Rights Reserved</span>
        <div id="payment-options">
            <span>We accept these payment methods:</span>
            <br />
            <br />
            <img src="../Images/payment-options.png" alt="Payment-options" />
        </div>
        <div class="clear"></div>
    </footer>';
    }
    public static function EchoSharedLinks()
    {
        echo '<link rel="stylesheet" type="text/css" href="../Styles/StaticStyle.css"/>';
        echo '<link rel="stylesheet" type="text/css" href="../Styles/ItemDivStyle.css"/>';
        echo '<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" integrity="sha384-50oBUHEmvpQ+1lW4y57PTFmhCaXp0ML5d60M1M7uH2+nqUivzIebhndOJK28anvf" crossorigin="anonymous">';
        echo '<meta name="viewport" content="width=device-width, initial-scale=1">';
        echo '<script type="text/javascript" src="../Scripts/StaticScript.js"></script>';
    }
}
