<?php

namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\BOrder.php');

use AmazonV2\ConnectionFactory;
use AmazonV2\Models\BOrder;
use PDOStatement;
use DateTime;
use PDO;

class BOrderRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    private function mapBOrder(PDOStatement $stmt): array
    {
        $result = [];
        while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $row_BOrder = new BOrder();
            $row_BOrder->id = intval($row["id"]);
            $row_BOrder->orderDate = new DateTime();
            $row_BOrder->orderDate->setTimestamp(intval($row["orderDate"]));
            $row_BOrder->shippingAddress = $row["shippingAddress"];
            $row_BOrder->total = floatval($row["total"]);
            $row_BOrder->customerId = intval($row["customerId"]);
            $result[] = $row_BOrder;
        }
        return $result;
    }
    public function get(int $id): ?BOrder
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM BOrder WHERE id = :id");
        $stmt->bindParam(":id", $id);
        $stmt->execute();
        $result = $this->mapBOrder($stmt)[0];

        $con = null;
        return $result;
    }
    public function getByCustomer(int $customerId): ?array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM BOrder WHERE customerId = :customerId ORDER BY id DESC");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();
        $result = $this->mapBOrder($stmt);

        $con = null;
        return $result;
    }

    public function create(BOrder &$order): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO BOrder(orderDate, shippingAddress, total, customerId) VALUES(:orderDate, :shippingAddress, CAST(:total as decimal(20,4)), :customerId)");
        $stmt->bindParam(":orderDate", $order->orderDate->getTimestamp());
        $stmt->bindParam(":shippingAddress", $order->shippingAddress);
        $stmt->bindParam(":total", $order->total);
        $stmt->bindParam(":customerId", $order->customerId);
        $stmt->execute();

        $order->id = (int)$con->lastInsertId();
        $con = null;
    }
    public function updateTotal(int $bOrderId, float $total): void
    {
        echo $total;
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("UPDATE BOrder SET total = CAST(:total as decimal(20,4)) WHERE id = :bOrderId");
        $stmt->bindParam(":bOrderId", $bOrderId);
        $stmt->bindValue(":total", $total);
        $stmt->execute();
        $con = null;
    }
}
