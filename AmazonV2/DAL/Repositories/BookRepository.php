<?php

namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\Book.php');

use AmazonV2\ConnectionFactory;
use AmazonV2\Models\Book;
use PDOStatement;
use PDO;
use DateTime;

class BookSearchContext
{
    //Set the fields that you don't want to be included to NULL
    public $namePattern;
    public $maxUnitPrice;
    public $minAge;
    public $maxAge;
    public $categoriesIds;

    public $offset = 0;
    public $count = 9999;

    public $orderBySoldQuantity;
    public $orderByViews;
    public $orderByUnitPrice;
    public $orderByRating;
}
class BookRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    private function mapBook(PDOStatement $stmt): array
    {
        $result = [];
        while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $row_Book = new Book();
            $row_Book->id = $row["id"];
            $row_Book->name = $row["name"];
            $row_Book->views = intval($row["views"]);
            $row_Book->unitPrice = floatval($row["unitPrice"]);
            $row_Book->soldQuantity = intval($row["soldQuantity"]);
            $row_Book->quantity = intval($row["quantity"]);
            $row_Book->description = $row["description"];
            $row_Book->authorId = intval($row["authorId"]);
            $row_Book->minAge = intval($row["minAge"]);
            $row_Book->maxAge = intval($row["maxAge"]);
            $row_Book->numberOfPages = intval($row["numberOfPages"]);
            $row_Book->coverType = $row["coverType"];
            $row_Book->publisher = $row["publisher"];
            $row_Book->publicationLocation = $row["publicationLocation"];
            $row_Book->publicationDate = new DateTime();

            $row_Book->publicationDate->setTimestamp(intval($row["publicationDate"]));
            $row_Book->rating = floatval($row["rating"]);
            $row_Book->rating1 = floatval($row["rating1"]);
            $row_Book->rating2 = floatval($row["rating2"]);
            $row_Book->rating3 = floatval($row["rating3"]);
            $row_Book->rating4 = floatval($row["rating4"]);
            $row_Book->rating5 = floatval($row["rating5"]);
            $row_Book->ratersCount = intval($row["ratersCount"]);
            $result[] = $row_Book;
        }
        return $result;
    }

    public function get(int $bookId): ?Book
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT bk.* FROM Book bk WHERE bk.id = :bookId");
        $stmt->bindParam(":bookId", $bookId);
        $stmt->execute();

        $result = $this->mapBook($stmt)[0];

        $con = null;
        return $result;
    }
    public function getAll(): array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Book");
        $stmt->execute();
        $result = $this->mapBook($stmt);

        $con = null;
        return $result;
    }
    public function getPrice(int $bookId): float
    {
        $act = (new DateTime(\DateTimeZone::ALL))->getTimestamp();
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare(";WITH BooksDiscounts (bookId, unitPrice) AS(
        SELECT bookId, unitPrice FROM Discount WHERE startTime <= :act1 AND endTime >= :act2 AND bookId = :bookId1)
        SELECT COALESCE(dis.unitPrice, bk.unitPrice) FROM Book bk
        LEFT OUTER JOIN BooksDiscounts dis
        ON dis.bookId = bk.id
        WHERE bk.id = :bookId2");
        $stmt->bindValue(":bookId1", $bookId);
        $stmt->bindValue(":bookId2", $bookId);
        $stmt->bindValue(":act1", $act);
        $stmt->bindValue(":act2", $act);
        $stmt->execute();
        $result = floatval($stmt->fetchColumn());

        $con = null;
        return $result;
    }
    //Used when selling a book
    public function decrementQuantity(int $bookId, int $quantityUpdate): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("UPDATE Book SET quantity = quantity - :quantityUpdate1, soldQuantity = soldQuantity + :quantityUpdate2 WHERE id = :bookId");
        $stmt->bindParam(":bookId", $bookId);
        $stmt->bindParam(":quantityUpdate1", $quantityUpdate);
        $stmt->bindParam(":quantityUpdate2", $quantityUpdate);
        $stmt->execute();

        $con = null;
    }

    //used when a book PAGE is requested
    public function incrementViews(int $bookId, int $viewsUpdate = 1): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("UPDATE Book SET views = views + :viewsUpdate WHERE id = :bookId");
        $stmt->bindValue(":bookId", $bookId);
        $stmt->bindValue(":viewsUpdate", $viewsUpdate);
        $stmt->execute();

        $con = null;
    }
    private function swap(&$x, &$y)
    {
        $tmp = $x;
        $x = $y;
        $y = $tmp;
    }

    public function search(BookSearchContext $ctx): ?array
    {

        $con = $this->connectionFactory->getConnection();
        $params = [":offset" => strval($ctx->offset), ":count" => strval($ctx->count), ":namePattern" => "%", ":namePattern1" => "%"];
        $condition = "";
        $split = false;
        if ($ctx->namePattern != null) {
            $params[":namePattern"] = $ctx->namePattern;
            $params[":namePattern1"] = $ctx->namePattern;
        }
        if ($ctx->minAge != null) {
            if ($split) {
                $condition .= " AND ";
            }
            $condition .= "bk.minAge >= :minAge";
            $params[":minAge"] = $ctx->minAge;
            $split = true;
        }
        if ($ctx->maxAge != null) {
            if ($split) {
                $condition .= " AND ";
            }
            $condition .= "bk.maxAge <= :maxAge1";
            $params[":maxAge1"] = $ctx->maxAge;
            //$params[":maxAge2"] = $ctx->maxAge;
            $split = true;
        }
        if ($ctx->maxUnitPrice != null) {
            if ($split) {
                $condition .= " AND ";
            }
            $condition .= "bk.unitPrice <= :maxUnitPrice";
            $params[":maxUnitPrice"] = $ctx->maxUnitPrice;
            $split = true;
        }

        $categoriesCondition = "";
        if ($ctx->categoriesIds != null) {
            $categoriesArray = "";
            $splitCategoriesArray = false;
            $categoriesCount = 0;
            foreach ($ctx->categoriesIds as $cat) {
                if ($splitCategoriesArray) {
                    $categoriesArray .= " , ";
                }
                $splitCategoriesArray = true;
                $categoriesArray .= $cat;
                $categoriesCount += 1;
            }
            $categoriesCondition = "categoryId IN ($categoriesArray) GROUP BY bookId HAVING (COUNT(categoryId) >= CAST(:categoriesCount AS INT))";
            $params[":categoriesCount"] = $categoriesCount;
        } else {
            $categoriesCondition = "1 = 1";
        }
        if (strlen($condition) == 0) {
            $condition = "1 = 1";
        }
        $orderColumns = ["bk.soldQuantity DESC", "bk.views DESC", "bk.unitPrice ASC", "bk.rating DESC"];
        if ($ctx->orderByUnitPrice) {
            $this->swap($orderColumns[0], $orderColumns[2]);
        }
        if ($ctx->orderByViews) {
            $this->swap($orderColumns[0], $orderColumns[1]);
        }
        if ($ctx->orderByRating) {
            $this->swap($orderColumns[0], $orderColumns[3]);
        }
        $orderColumnsString = "(CASE WHEN bk.quantity > 0 THEN 0 ELSE 1 END), {$orderColumns[0]}, {$orderColumns[1]}, {$orderColumns[2]}, {$orderColumns[3]}";

        $categoriesCTESelectStatement = "SELECT id FROM Book";
        if ($ctx->categoriesIds !== null) {
            $categoriesCTESelectStatement = "SELECT bookId AS id FROM bookCategory WHERE $categoriesCondition";
        }
        //todo use limit here
        $stmt = $con->prepare(";WITH CategoriesBooks (id) AS
        ($categoriesCTESelectStatement)
        SELECT bk.* FROM Book bk
        WHERE ($condition) 
        AND EXISTS (SELECT * FROM CategoriesBooks cb WHERE cb.id = bk.id)
        AND (UPPER(bk.name) LIKE UPPER(:namePattern) OR EXISTS (SELECT name from author au where au.id = bk.authorId and upper(au.name) like UPPER(:namePattern1)))
        ORDER BY $orderColumnsString LIMIT CAST(:count AS INT) OFFSET CAST(:offset as INT)");
        foreach ($params as $paramName => $paramValue) {
            $stmt->bindValue($paramName, $paramValue);
        }
        $stmt->execute();
        $result =  $this->mapBook($stmt);

        $con = null;
        return $result;
    }
}
