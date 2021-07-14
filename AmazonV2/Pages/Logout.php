<?php
use AmazonV2\UserSystem\LoginManager;

require_once(dirname(__FILE__, 2) . '\Pages\StaticPage.php');
session_start();
LoginManager::Logout();
session_destroy();

header('Location: HomePage.php', true, 303);
