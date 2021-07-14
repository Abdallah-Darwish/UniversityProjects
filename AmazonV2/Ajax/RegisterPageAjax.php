<?php
require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
use AmazonV2\Repositories\RepositoriesFactory;

$customerRepo = RepositoriesFactory::getCustomerRepository();
if (array_key_exists("email", $_REQUEST)) {
    $email = $_REQUEST["email"];
    echo $customerRepo->emailExists($email) ? "false" : "true";
} else if (isset($_REQUEST["userName"])) {
    $userName = $_REQUEST["userName"];
    echo $customerRepo->userNameExists($userName) ? "false" : "true";
} else {
    echo "false";
}
