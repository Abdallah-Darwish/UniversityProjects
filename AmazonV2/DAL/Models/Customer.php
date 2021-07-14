<?php
namespace AmazonV2\Models;

class Customer
{
    public $id;
    public $firstName;
    public $lastName;
    public $userName;
    public $email;
    public $passwordHash;
    public $addressLine1;
    public $addressLine2;
    public $country;
    public $city;
    public $zipCode;
    public $creditCardType;
    public $creditCardNumber;
    public $creditCardName;
    public $creditCardExpiryDate;

    public $telephone;
    public $birthday;
    //string
    public $gender;
    public $registrationDate;
}
