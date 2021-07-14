<?php
namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\WishListItem.php');

use AmazonV2\Models\WishListItem;
use PDOStatement;
use PDO;
use AmazonV2\ConnectionFactory;

class WishListItemRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    public function getByCustomer(int $customerId): ?array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM WishListItem WHERE customerId = :customerId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\WishListItem");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function isInCustomerWishList(int $customerId, int $bookId): bool
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT id FROM WishListItem WHERE customerId = :customerId AND bookId = :bookId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->bindParam(":bookId", $bookId);
        $stmt->execute();

        $result =  $stmt->fetchColumn() !== false;

        $con = null;
        return $result;
    }
    public function create(WishListItem &$item): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO WishListItem(bookId, customerId) VALUES(:bookId, :customerId)");
        $stmt->bindParam(":customerId", $item->customerId);
        $stmt->bindParam(":bookId", $item->bookId);
        $stmt->execute();

        $item->id = (int)$con->lastInsertId();

        $con = null;
    }
    public function deleteFromCustomerWishList(int $customerId, int $bookId): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("DELETE FROM WishListItem WHERE customerId = :customerId AND bookId = :bookId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->bindParam(":bookId", $bookId);
        $stmt->execute();

        $con = null;
    }
}
