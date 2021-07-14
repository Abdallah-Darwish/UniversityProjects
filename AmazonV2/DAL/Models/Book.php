<?php
namespace AmazonV2\Models;

class Book
{
    public $id;
    public $unitPrice;
    public $name;
    public $views;
    public $soldQuantity;
    public $quantity;
    public $description;
    public $authorId;
    public $minAge;
    public $maxAge;
    public $numberOfPages;
    //STRING
    public $coverType;
    public $publisher;
    public $publicationLocation;
    public $publicationDate;
    //[0 , 5]
    public $rating;
    public $rating1;
    public $rating2;
    public $rating3;
    public $rating4;
    public $rating5;
    //Number of users who had rated the book
    public $ratersCount;
}
