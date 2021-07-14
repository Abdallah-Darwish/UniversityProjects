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
const PAGE_SIZE = 20;
LoginManager::loginUsingCookie();

$booksRepo = RepositoriesFactory::getBookRepository();
$categoryRepo = RepositoriesFactory::getCategoryRepository();
$ctx = new BookSearchContext();
$page = -1;
if (array_key_exists("page", $_REQUEST)) 
{
    $page = intval($_REQUEST["page"]);
}
else
{
    $page = 1;
}
$page = intval($_REQUEST["page"]);
$ctx->orderBySoldQuantity = true;
$ctx->offset = $page * PAGE_SIZE + 1;
$ctx->count = PAGE_SIZE;
$bestSellers = $booksRepo->search($ctx);
$bestCategories = $categoryRepo->getBest(10);

if (count($bestSellers) === 0) {
    $page--;
    header("Location: HomePage.php?page=$page", true, 303);
    exit();
}
?>
<!DOCTYPE html>
<html>

<head>
    <title>AmazonV2</title>
    <?php StaticPage::EchoSharedLinks() ?>
    <link rel="stylesheet" type="text/css" href="../Styles/SearchPageStyle.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/HomePageStyle.css" />
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <div class="clear"></div>
    <div id="search">
        <form action="SearchPage.php" method="GET">
            <img style="height:120px;" src="../Images/Books.jpg" />
            <input type="search" name="name" id="txtSearch" placeholder="Search" />
        </form>
    </div>

    <div id="Main">
        <div id="Categories">
            <div>Shop by category</div>

            <ul>
                <?php
                foreach ($bestCategories as $cat) {
                    echo "<li><a href=\"SearchPage.php?category[]=$cat->id\">$cat->name</a></li>\n";
                }
                ?>
            </ul>
        </div>
        <div id="middle">

            <div id="div1" style="width: 100%;"> Best Sellers</div>
            <div class="clear"></div>
            <div id="page1">
                <div class="pages">
                    <?php
                    foreach ($bestSellers as $book) {
                        StaticPage::echoBookDiv($book);
                    }
                    ?>
                    <div class="clear"></div>
                </div>
            </div>
            <nav style="font-size:large;">
                <a href="HomePage.php?page=<?= $page - 1 ?>" target="_top" onclick="return <?= $page === 0 ? "false;" : "true" ?>">Previous</a>
                <a href="HomePage.php?page=<?= $page + 1 ?>" target="_top" onclick="return <?= count($bestSellers) !== PAGE_SIZE ? "false;" : "true" ?>">Next</a>
            </nav>
        </div>
        <div class="clear"></div>

    </div>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>