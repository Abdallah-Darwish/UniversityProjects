<?php
namespace AmazonV2\UserSystem;

require_once(dirname(__FILE__, 2) . '\DAL\RepositoriesFactory.php');
use AmazonV2\Models\Customer;
use AmazonV2\Repositories;
use AmazonV2\Repositories\RepositoriesFactory;
use Exception;

class LoginManager
{
    public const CustomerKey = "CUSTOMER_ID";
    public const RememberCookieName = "RememberMe";
    public const RememberCookieExpiryTime = 7 * 24 * 60 * 60;
    private static function hashPassword(string $password): string
    {
        return base64_encode($password);
        //return $password;
    }

    //Fill passwordHash with the password and the manager will hash-it
    public static function register(Customer &$customer): void
    {
        $customer->passwordHash = self::hashPassword($customer->passwordHash);

        $customersRepo = RepositoriesFactory::getCustomerRepository();
        $customersRepo->create($customer);
    }
    public static function isLoggedIn(): bool
    {
        return array_key_exists(self::CustomerKey, $_SESSION);
    }
    public static function getLoggedInCustomer(): Customer
    {
        return RepositoriesFactory::getCustomerRepository()->get(intval($_SESSION[self::CustomerKey]));
    }
    public static function login(string $userName, string $password, bool $remember)
    {
        $customersRepo = RepositoriesFactory::getCustomerRepository();
        $customer = $customersRepo->getByUserName($userName);
        if ($customer === null) {
            throw new Exception("Username doesn't exist.");
        }
        if ($customer->passwordHash !== self::hashPassword($password)) {
            throw new Exception("Invalid password.");
        }
        $_SESSION[self::CustomerKey] = $customer->id;
        if ($remember) {
            $cookieValue = base64_encode((string)$customer->id);
            setcookie(self::RememberCookieName, $cookieValue, time() + self::RememberCookieExpiryTime, '/');
        }
    }
    public static function changePassword(int $customerId, string $password)
    {
        $customersRepo = RepositoriesFactory::getCustomerRepository();
        $customer = $customersRepo->get($customerId);
        $customer->passwordHash = self::hashPassword($password);
        $customersRepo->update($customer);
    }
    public static function loginUsingCookie(): bool
    {
        if (self::isLoggedIn()) {
            return true;
        }
        if (array_key_exists(self::RememberCookieName, $_COOKIE) === false) {
            return false;
        }
        $cookieValue = $_COOKIE[self::RememberCookieName];
        $cookieValue = base64_decode($cookieValue);
        $customerId = intval($cookieValue);
        $customersRepo = RepositoriesFactory::getCustomerRepository();
        $customer = $customersRepo->get($customerId);
        if ($customer == null) {
            return false;
        }
        $_SESSION[self::CustomerKey] = $customer->id;
        return true;
    }
    public static function logout()
    {
        if (self::isLoggedIn()) {
            unset($_SESSION[self::CustomerKey]);
            if (isset($_COOKIE[self::RememberCookieName])) {
                unset($_COOKIE[self::RememberCookieName]);
                setcookie(self::RememberCookieName, '', 1, '/');
            }
        }
    }
    public static function isAdmin(): bool
    {
        if (self::isLoggedIn() === false) {
            return false;
        }
        return strtoupper(self::getLoggedInCustomer()->userName) === "ADMINISTRATOR";
    }
}
