<?php

namespace AmazonV2;

use PDO;

class ConnectionFactory
{
    private $connectionString;
    public function __construct(string $conString)
    {
        $this->connectionString = $conString;
    }
    public function getConnection(): PDO
    {
        $con = new PDO($this->connectionString);
        $con->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $con->setAttribute(PDO::ATTR_CASE, PDO::CASE_NATURAL);
        return $con;
    }
}
