<?php
use AmazonV2\Models\Book;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\BookRating;
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Repositories\BookRepository;
use AmazonV2\Repositories\RepositoriesFactory;
use AmazonV2\Repositories\BookSearchContext;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();
const PAGE_SIZE = 24;
LoginManager::loginUsingCookie();

$page = intval($_REQUEST["page"]);
$selectedCategories = $_REQUEST["category"];
$name = $_REQUEST["name"];
$maxAge = $_REQUEST["maxAge"];
$maxUnitPrice = $_REQUEST["maxUnitPrice"];
$orderBy = $_REQUEST["orderBy"];
$searchResult = [];
$booksRepo = RepositoriesFactory::getBookRepository();
$ctx = new BookSearchContext();
$ctx->categoriesIds = $selectedCategories;
$ctx->offset = $page * PAGE_SIZE;
$ctx->count = PAGE_SIZE;
$ctx->maxAge = $maxAge;
$ctx->maxUnitPrice = $maxUnitPrice;
if (strlen($name) > 0) {
    $ctx->namePattern = '%' . $name . '%';
}

if ($orderBy === "unitPrice") {
    $ctx->orderByUnitPrice = true;
} else if ($orderBy === "views") {
    $ctx->orderByViews = true;
} else if ($orderBy === "soldQuantity") {
    $ctx->orderBySoldQuantity = true;
} else if ($orderBy === "rating") {
    $ctx->orderByRating = true;
}

$searchResult = $booksRepo->search($ctx);
if ($selectedCategories === null) {
    $selectedCategories = [];
}
?>
<!DOCTYPE html>
<html>

<head>
    <title>Google V2</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/SearchPageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div id="Main">
        <form action="" method="GET">
            <div id="searchName">
                <h2> Enter a book's Name or an Author's Name</h2>
                <input type="text" name="name" value="<?= $name ?>" />
            </div>

            <div id="Categories">
                <h2>Shop by category</h2>
                <ul>
                    <?php
                    $categories = RepositoriesFactory::getCategoryRepository()->getAll(0, 99999);
                    foreach ($categories as $category) {
                        $checked = in_array($category->id, $selectedCategories) ? "checked" : "";
                        $checkboxId = "cat" . $category->id;
                        echo "<li><input id=\"$checkboxId\" type=\"checkbox\" name=\"category[]\" value=\"$category->id\" $checked/><label for=\"$checkboxId\">$category->name</li>";
                    }
                    ?>
                </ul>
            </div>
            <div id="orderBy">
                <h2>Order result by</h2>
                <select name="orderBy">
                    <option value="unitPrice" <?= $orderBy === "unitPrice" ? "selected" : "" ?>>Price</option>
                    <option value="views" <?= $orderBy === "views" ? "selected" : "" ?>>Views</option>
                    <option value="soldQuantity" <?= $orderBy === "soldQuantity" ? "selected" : "" ?>>Sold quantity</option>
                    <option value="rating" <?= $orderBy === "rating" ? "selected" : "" ?>>Rating</option>
                </select>
                <br />
                <br />
                <button>Search</button>
            </div>

            <div id="words">
                <br />
                <h1>Search for your book in any way that you desire!</h1>
                <img src="../Images/choose.jpg" />
            </div>
            <div class="clear"></div>

            <div id="ageRange">
                <h2>Enter the age range you desire</h2>
                <input type="number" name="maxAge" value="<?= $maxAge ?>" />
            </div>

            <div class="clear"></div>

            <div id="priceRange">
                <h2>Enter the price range you desire</h2>
                <input type="number" name="maxUnitPrice" value="<?= $maxUnitPrice ?>" step="0.01" />
            </div>
            <div class="clear"></div>
        </form>


        <div id="searchResults">
            <h2>Search Result</h2>
            <?php
            foreach ($searchResult as $book) {
                StaticPage::echoBookDiv($book);
            }
            ?>
            <div class="clear"></div>
            <nav style="font-size:large;">
                <?php
                $queryString = "SearchPage.php?maxAge={$_REQUEST["maxAge"]}&maxUnitPrice={$_REQUEST["maxUnitPrice"]}&name={$_REQUEST["name"]}&orderBy={$_REQUEST["orderBy"]}&";
                foreach ($selectedCategories as $cat) {
                    $queryString .= "category[]=$cat&";
                }
                $queryString .= "page="
                ?>
                <a href="<?= $queryString . ($page - 1) ?>" target="_top" onclick="return <?= $page === 0 ? "false;" : "true" ?>">Previous</a>
                <a href="<?= $queryString . ($page + 1) ?>" target="_top" onclick="return <?= count($searchResult) !== PAGE_SIZE ? "false;" : "true" ?>">Next</a>
            </nav>
        </div>
    </div>
    <div class="clear"></div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>