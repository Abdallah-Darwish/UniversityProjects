<?php
use AmazonV2\UserSystem\LoginManager;
use AmazonV2\Pages\StaticPage;
use AmazonV2\Models\Discount;
use AmazonV2\Repositories\RepositoriesFactory;
use DateTimeZone;
use DateTime;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();

LoginManager::loginUsingCookie();
if (LoginManager::isLoggedIn() === false || LoginManager::isAdmin() === false) {
    header('Location: LoginPage.php', true, 303);
    exit();
}
if (array_key_exists("bookId", $_REQUEST)) {
    $discountRepository = RepositoriesFactory::getDiscountRepository();
    $discount = new Discount();
    $discount->bookId = $_REQUEST["bookId"];
    $discount->unitPrice = floatval($_REQUEST["unitPrice"]);
    $discount->startTime = new DateTime($_REQUEST["startTime"]);
    $discount->endTime = new DateTime($_REQUEST["endTime"]);
    $discountRepository->create($discount);
    echo $discount->id;
}
?>
<!DOCTYPE html>

<html>

<head>
    <title>I AM SO TIRED</title>
    <?php StaticPage::EchoSharedLinks() ?>
</head>

<body onload="setup();">
    <?php StaticPage::EchoHeader(); ?>

    <div class="clear"></div>
    <center>
        <form action="AdminDiscountPage.php" method="GET">
            <fieldset>
                <legend>New Discount Info</legend>
                <label for="lstBooks">Book</label>
                <select id="lstBooks" name="bookId" required>
                    <?php
                    $bookRepo = RepositoriesFactory::getBookRepository();
                    $books = $bookRepo->getAll();
                    foreach ($books as $book) {
                        echo "<option value=\"$book->id\">$book->name</option>\n";
                    }
                    ?>
                </select>
                <br>
                <br>
                <label for="txtUnitPrice">Unit Price</label>
                <input id="txtUnitPrice" type="number" name="unitPrice" step="0.001" /> <br>
                <br>
                <label for="dteStartTime">Start time: </label>
                <input id="dteStartTime" type="date" min="<?= date('Y-m-d') ?>" name="startTime" />
                <br>
                <br>
                <label for="dteEndTime">End time: </label>
                <input id="dteEndTime" type="date" min="<?= date('Y-m-d') ?>" name="endTime" />
                <br>
                <br>
                <input type="submit" value="Create" />
            </fieldset>
        </form>
    </center>

</body>

</html>