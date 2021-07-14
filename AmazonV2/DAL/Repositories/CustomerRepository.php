<?php

namespace AmazonV2\Repositories;

require_once(dirname(__FILE__, 2) . '\ConnectionFactory.php');
require_once(dirname(__FILE__, 2) . '\Models\Customer.php');

use AmazonV2\Models\Customer;
use AmazonV2\ConnectionFactory;
use PDO;
use PDOStatement;
use DateTime;

class CustomerRepository
{
    private $connectionFactory;
    public function __construct(ConnectionFactory $fac)
    {
        $this->connectionFactory = $fac;
    }
    private function mapCustomer(PDOStatement $stmt): array
    {
        $result = [];
        while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $row_customer = new Customer();
            $row_customer->id = (int)$row["id"];
            $row_customer->firstName = $row["firstName"];
            $row_customer->lastName = $row["lastName"];
            $row_customer->email = $row["email"];
            $row_customer->passwordHash = $row["passwordHash"];
            $row_customer->userName = $row["userName"];
            $row_customer->addressLine1 = $row["addressLine1"];
            $row_customer->addressLine2 = $row["addressLine2"];
            $row_customer->city = $row["city"];
            $row_customer->country = $row["country"];
            $row_customer->zipCode = $row["zipCode"];
            $row_customer->creditCardType = $row["creditCardType"];
            $row_customer->creditCardExpiryDate = new DateTime();
            $row_customer->creditCardExpiryDate->setTimestamp(intval($row["creditCardExpiryDate"]));
            $row_customer->creditCardName = $row["creditCardName"];
            $row_customer->creditCardNumber = $row["creditCardNumber"];
            $row_customer->telephone = $row["telephone"];
            $row_customer->birthday = new DateTime();
            $row_customer->birthday->setTimestamp(intval($row["birthday"]));
            $row_customer->gender = $row["gender"];
            $row_customer->registerDate = new DateTime();
            $row_customer->registerDate->setTimestamp(intval($row["registrationDate"]));

            $result[] = $row_customer;
        }
        return $result;
    }
    public function get(int $customerId): ?Customer
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Customer WHERE id = :customerId");
        $stmt->bindParam(":customerId", $customerId);
        $stmt->execute();

        $result = $this->mapCustomer($stmt)[0];

        $con = null;
        return $result;
    }
    public function getByUserName(string $userName): ?Customer
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT * FROM Customer WHERE userName = :userName");
        $stmt->bindParam(":userName", $userName);
        $stmt->execute();

        $result = $this->mapCustomer($stmt)[0];

        $con = null;
        return $result;
    }
    public function create(Customer &$customer): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("INSERT INTO Customer(firstName, lastName, userName, passwordHash, email, addressLine1, addressLine2, city, country, zipCode, creditCardType, creditCardNumber, creditCardName, creditCardExpiryDate, telephone, birthday, gender, registrationDate) VALUES(:firstName, :lastName, :userName, :passwordHash, :email, :addressLine1, :addressLine2, :city, :country, :zipCode, :creditCardType, :creditCardNumber, :creditCardName, :creditCardExpiryDate, :telephone, :birthday, :gender, :registrationDate)");
        $stmt->bindParam(":firstName", $customer->firstName);
        $stmt->bindParam(":lastName", $customer->lastName);
        $stmt->bindParam(":userName", $customer->userName);
        $stmt->bindParam(":passwordHash", $customer->passwordHash);
        $stmt->bindParam(":email", $customer->email);
        $stmt->bindParam(":addressLine1", $customer->addressLine1);
        $stmt->bindParam(":addressLine2", $customer->addressLine2);
        $stmt->bindParam(":city", $customer->city);
        $stmt->bindParam(":country", $customer->country);
        $stmt->bindParam(":zipCode", $customer->zipCode);
        $stmt->bindParam(":creditCardType", $customer->creditCardType);
        $stmt->bindParam(":creditCardNumber", $customer->creditCardNumber);
        $stmt->bindValue(":creditCardExpiryDate", $customer->creditCardExpiryDate->getTimestamp());
        $stmt->bindParam(":creditCardName", $customer->creditCardName);
        $stmt->bindParam(":telephone", $customer->telephone);
        $stmt->bindValue(":birthday", $customer->birthday->getTimestamp());
        $stmt->bindParam(":gender", $customer->gender);
        $stmt->bindValue(":registrationDate", $customer->registrationDate->getTimestamp());
        $stmt->execute();

        $customer->id = (int)$con->lastInsertId();

        $con = null;
    }
    //WONT UPDATE USERNAME OR EMAIL
    public function update(Customer $customer): void
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare(
            "UPDATE Customer SET 
            firstName = :firstName,
            lastName = :lastName,
            passwordHash = :passwordHash,
            addressLine1 = :addressLine1,
            addressLine2 = :addressLine2,
            city = :city,
            country = :country,
            zipCode = :zipCode,
            creditCardType = :creditCardType,
            creditCardNumber = :creditCardNumber,
            creditCardExpiryDate = :creditCardExpiryDate,
            creditCardName = :creditCardName,
            telephone = :telephone,
            birthday = :birthday,
            gender = :gender
            WHERE id = :id"
        );
        $stmt->bindParam(":id", $customer->id);
        $stmt->bindParam(":firstName", $customer->firstName);
        $stmt->bindParam(":lastName", $customer->lastName);
        $stmt->bindParam(":passwordHash", $customer->passwordHash);
        $stmt->bindParam(":addressLine1", $customer->addressLine1);
        $stmt->bindParam(":addressLine2", $customer->addressLine2);
        $stmt->bindParam(":city", $customer->city);
        $stmt->bindParam(":country", $customer->country);
        $stmt->bindParam(":zipCode", $customer->zipCode);
        $stmt->bindParam(":creditCardType", $customer->creditCardType);
        $stmt->bindParam(":creditCardNumber", $customer->creditCardNumber);
        $stmt->bindValue(":creditCardExpiryDate", $customer->creditCardExpiryDate->getTimestamp());
        $stmt->bindParam(":creditCardName", $customer->creditCardName);
        $stmt->bindParam(":telephone", $customer->telephone);
        $stmt->bindValue(":birthday", $customer->birthday->getTimestamp());
        $stmt->bindParam(":gender", $customer->gender);
        $stmt->execute();

        $con = null;
    }

    public function emailExists(string $email): bool
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT id FROM Customer WHERE UPPER(email) = :email ORDER BY id OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY");
        $stmt->bindParam(":email", $email);
        $stmt->execute();

        $result = $stmt->fetchColumn() !== false;
        $con = null;
        return $result;
    }
    public function userNameExists(string $userName): bool
    {
        $con = $this->connectionFactory->getConnection();
        $stmt = $con->prepare("SELECT id FROM Customer WHERE UPPER(userName) = UPPER(:userName) ORDER BY id OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY");
        $stmt->bindParam(":userName", $userName);
        $stmt->execute();

        $result = $stmt->fetchColumn() !== false;
        $con = null;
        return $result;
    }
}
