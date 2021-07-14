<?php
namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\CartItem.php');

use AmazonV2\Models\CartItem;
use AmazonV2\ConnectionFactory;
use PDO;
use PDOStatement;
use DateTime;

class CartItemRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    public function create(CartItem &$cartItem): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO CartItem(customerId, bookId, quantity) VALUES(:customerId, :bookId, :quantity)");
        $stmt->bindParam(":customerId", $cartItem->customerId);
        $stmt->bindParam(":bookId", $cartItem->bookId);
        $stmt->bindParam(":quantity", $cartItem->quantity);
        $stmt->execute();

        $cartItem->id = (int)$con->lastInsertId();

        $con = null;
    }

    public function delete(int $itemId): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("DELETE FROM CartItem WHERE id = :itemId");
        $stmt->bindParam(":itemId", $itemId);
        $stmt->execute();

        $con = null;
    }
    public function deleteByBookId(int $customerId, int $bookId)
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("DELETE FROM CartItem WHERE customerId = :customerId AND bookId = :bookId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->bindParam(":bookId", $bookId);
        $stmt->execute();

        $con = null;
    }
    public function get(int $itemId): ?CartItem
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM CartItem WHERE id = :itemId");
        $stmt->bindParam(":itemId", $itemId);
        $stmt->execute();

        $result =  $stmt->fetchObject("AmazonV2\Models\CartItem");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function getByBookId(int $customerId, int $bookId): ?CartItem
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM CartItem WHERE bookId = :bookId AND customerId = :customerId");
        $stmt->bindParam(":bookId", $bookId);
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();

        $result =  $stmt->fetchObject("AmazonV2\Models\CartItem");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function updateQuantity(int $itemId, int $newQuantity): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("UPDATE CartItem SET quantity = :newQuantity WHERE id = :itemId");
        $stmt->bindParam(":newQuantity", $newQuantity);
        $stmt->bindParam(":itemId", $itemId);
        $stmt->execute();

        $con = null;
    }
    public function getCartTotal(int $customerId): float
    {
        $act = (new DateTime(\DateTimeZone::ALL))->getTimestamp();
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare(";WITH BooksDiscounts (bookId, unitPrice) AS(
            SELECT bookId, unitPrice FROM Discount WHERE startTime <= :act1 AND endTime >= :act2)      
            ,BooksPrices(id, unitPrice) As(
            SELECT bk.id, COALESCE(dis.unitPrice, bk.unitPrice) as unitPrice FROM Book bk
            LEFT OUTER JOIN BooksDiscounts dis
            ON dis.bookId = bk.id)
            SELECT SUM(ci.quantity * bk.unitPrice) FROM CartItem ci
            INNER JOIN BooksPrices bk
            ON bk.id = ci.bookId
            WHERE ci.customerId = :customerId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->bindValue(":act1", $act);
        $stmt->bindValue(":act2", $act);
        $stmt->execute();

        $result = $stmt->fetchColumn();
        if ($result === false) {
            $result = 0.0;
        } else {
            $result = floatval($result);
        }
        $con = null;
        return $result;
    }
    public function getCartItemsCount(int $customerId): int
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT SUM(quantity) FROM CartItem WHERE customerId = :customerId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();

        $result = (int)$stmt->fetchColumn();
        $con = null;
        return $result;
    }
    public function getCustomerItems(int $customerId): array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM CartItem WHERE customerId = :customerId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\CartItem");
        if ($result === false) {
            $result = [];
        }
        $con = null;
        return $result;
    }
}
