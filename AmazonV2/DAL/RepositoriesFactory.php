<?php
namespace AmazonV2\Repositories;

require_once(__DIR__ . '/ConnectionFactory.php');
require_once(__DIR__ . '/Repositories/AuthorRepository.php');
require_once(__DIR__ . '/Repositories/BookRepository.php');
require_once(__DIR__ . '/Repositories/BOrderItemRepository.php');
require_once(__DIR__ . '/Repositories/BOrderRepository.php');
require_once(__DIR__ . '/Repositories/CartItemRepository.php');
require_once(__DIR__ . '/Repositories/CategoryRepository.php');
require_once(__DIR__ . '/Repositories/CustomerRepository.php');
require_once(__DIR__ . '/Repositories/DiscountRepository.php');
require_once(__DIR__ . '/Repositories/WishListItemRepository.php');
use AmazonV2\ConnectionFactory;

class RepositoriesFactory
{
    private static function getConnectionFactory(): ConnectionFactory
    {
        $dbPath = dirname(__FILE__, 2) . '/Database/Db.sqlite';
        $conFactory = new ConnectionFactory("sqlite:" . $dbPath);
        if(file_exists($dbPath) === false)
        {
            $con = $conFactory->getConnection();

            $dbInitScriptPath = dirname(__FILE__, 2) . '/Database/DbInit.sql';

            $dbInitScriptHandle = fopen($dbInitScriptPath, "r");
            $dbInitScript = fread($dbInitScriptHandle, filesize($dbInitScriptPath));
            fclose($dbInitScriptHandle);

            $con->exec($dbInitScript);
        }
        return $conFactory;
    }
    public static function getAuthorRepository(): AuthorRepository
    {
        return new AuthorRepository(self::getConnectionFactory());
    }
    public static function getBookRepository(): BookRepository
    {
        return new BookRepository(self::getConnectionFactory());
    }
    public static function getBOrderItemRepository(): BOrderItemRepository
    {
        return new BOrderItemRepository(self::getConnectionFactory());
    }
    public static function getBOrderRepository(): BOrderRepository
    {
        return new BOrderRepository(self::getConnectionFactory());
    }
    public static function getCartItemRepository(): CartItemRepository
    {
        return new CartItemRepository(self::getConnectionFactory());
    }
    public static function getCategoryRepository(): CategoryRepository
    {
        return new CategoryRepository(self::getConnectionFactory());
    }
    public static function getCustomerRepository(): CustomerRepository
    {
        return new CustomerRepository(self::getConnectionFactory());
    }
    public static function getDiscountRepository(): DiscountRepository
    {
        return new DiscountRepository(self::getConnectionFactory());
    }
    public static function getWishListItemRepository(): WishListItemRepository
    {
        return new WishListItemRepository(self::getConnectionFactory());
    }
}
