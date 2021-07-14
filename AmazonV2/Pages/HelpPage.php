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
LoginManager::loginUsingCookie();
?>
<!DOCTYPE html>
<html>

<head>
    <title>WE ARE THE ONES WHO NEED HELP</title>
    <?php StaticPage::EchoSharedLinks() ?>
</head>

<body>
    <?php StaticPage::EchoHeader(); ?>
    <p>
        This website was made by an under-paid, sleep-deprived students............so do you think we have a support team!!!
        <br>
        <br>
        <br>
        How did you even manage to break this shit in the first place?!!!
    </p>
    <?php StaticPage::EchoFooter(); ?>
</body>

</html>