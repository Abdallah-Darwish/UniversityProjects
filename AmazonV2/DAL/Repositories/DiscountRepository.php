<?php
namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\Discount.php');

use AmazonV2\Models\Discount;
use AmazonV2\ConnectionFactory;
use DateTime;
use PDOStatement;
use PDO;

class DiscountRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    private function mapDiscount(PDOStatement $stmt): array
    {
        $result = [];
        while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $row_discount = new Discount();
            $row_discount->id = intval($row["id"]);
            $row_discount->bookId = intval($row["bookId"]);
            $row_discount->unitPrice = floatval($row["unitPrice"]);
            $row_discount->startTime = new DateTime();
            $row_discount->startTime->setTimestamp(intval($row["startTime"]));
            $row_discount->endTime = new DateTime();
            $row_discount->endTime->setTimestamp(intval($row["endTime"]));
            $result[] = $row_discount;
        }
        return $result;
    }
    public function getFirstActive(int $bookId): ?Discount
    {
        $act = (new DateTime(\DateTimeZone::ALL))->getTimestamp();
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Discount ds WHERE ds.startTime <= :act1 AND ds.endTime >= :act2 AND ds.bookId = :bookId");
        $stmt->bindParam(":bookId", $bookId);
        $stmt->bindParam(":act1", $act);
        $stmt->bindParam(":act2", $act);
        $stmt->execute();

        $result = $this->mapDiscount($stmt)[0];
        $con = null;
        return $result;
    }
    public function create(Discount &$dis): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO Discount(bookId, unitPrice, startTime, endTime) VALUES(:bookId, :unitPrice, :startTime, :endTime)");
        $stmt->bindParam(":bookId", $dis->bookId);
        $stmt->bindParam(":unitPrice", $dis->unitPrice);
        $stmt->bindParam(":startTime", $dis->startTime->getTimestamp());
        $stmt->bindParam(":endTime", $dis->endTime->getTimestamp());
        $stmt->execute();

        $dis->id = $con->lastInsertId();
        $con = null;
    }
}
