<?php

namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\BOrderItem.php');

use AmazonV2\ConnectionFactory;
use AmazonV2\Models\BOrderItem;
use PDOStatement;
use PDO;

class BOrderItemRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    public function getByOrder(int $orderId): ?array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM BOrderItem  WHERE bOrderId = :orderId");
        $stmt->bindParam(":orderId", $orderId);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\BOrderItem");

        $con = null;
        return $result;
    }
    public function create(BOrderItem &$item): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO BOrderItem(bookId, quantity, unitPrice, bOrderId) VALUES(:bookId, :quantity, :unitPrice, :bOrderId)");
        $stmt->bindParam(":bookId", $item->bookId);
        $stmt->bindParam(":quantity", $item->quantity);
        $stmt->bindParam(":unitPrice", $item->unitPrice);
        $stmt->bindParam(":bOrderId", $item->bOrderId);
        $stmt->execute();

        $item->id = (int)$con->lastInsertId();

        $con = null;
    }
}
