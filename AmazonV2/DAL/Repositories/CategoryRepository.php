<?php

namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\Category.php');

use AmazonV2\Models\Category;
use AmazonV2\ConnectionFactory;
use PDO;
use PDOStatement;

class CategoryRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    public function get(int $id): ?Category
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Category WHERE id = :id");
        $stmt->bindParam(":id", $id);
        $stmt->execute();

        $result =  $stmt->fetchObject("AmazonV2\Models\Category");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function getAll(int $offset, int $count): ?array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Category ORDER BY id LIMIT CAST(:count as INT) OFFSET CAST(:offset as INT)");
        $stmt->bindParam(":offset", $offset);
        $stmt->bindParam(":count", $count);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\Category");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function getBest(int $count): ?array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Category cat ORDER BY (SELECT COUNT(id) FROM BookCategory bc WHERE bc.categoryId = cat.id) LIMIT CAST(:count as INT)");
        $stmt->bindParam(":count", $count);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\Category");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function create(Category &$cat): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO Category(name) VALUES(:name)");
        $stmt->bindParam(":name", $cat->name);
        $stmt->execute();

        $cat->id = $con->lastInsertId();
        $con = null;
    }

    public function update(Category $cat): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("UPDATE Category SET name = :name");
        $stmt->bindParam(":name", $cat->name);
        $stmt->execute();

        $con = null;
    }
    public function getByBook(int $bookId, int $count): array
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Category cat WHERE EXISTS(SELECT id FROM BookCategory bc WHERE bc.bookId = :bookId AND bc.categoryId = cat.id) ORDER BY id LIMIT CAST(:count AS INT)");
        $stmt->bindParam(":bookId", $bookId);
        $stmt->bindParam(":count", $count);
        $stmt->execute();

        $result =  $stmt->fetchAll(PDO::FETCH_CLASS, "AmazonV2\Models\Category");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
    public function delete(int $id): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("DELETE FROM BookCategory WHERE categoryId = :id1;
        DELETE FROM Category WHERE id = :id2;");
        $stmt->bindParam(":id1", $id);
        $stmt->bindParam(":id2", $id);
        $stmt->execute();

        $con = null;
    }
}
