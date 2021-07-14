<?php
namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\Models\Author.php');
require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');

use AmazonV2\Models\Author;
use AmazonV2\ConnectionFactory;
use PDOStatement;

class AuthorRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    public function get(int $authorId): ?Author
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Author WHERE id = :authorId");
        $stmt->bindParam(":authorId", $authorId);
        $stmt->execute();

        $result = $stmt->fetchObject("AmazonV2\Models\Author");
        if ($result === false) {
            $result = null;
        }
        $con = null;
        return $result;
    }
}
