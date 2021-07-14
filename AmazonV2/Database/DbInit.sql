CREATE TABLE Customer
(
    id INTEGER NOT NULL PRIMARY KEY,
    firstName TEXT NOT NULL,
    lastName TEXT NOT NULL,
    userName TEXT NOT NULL,
    passwordHash TEXT NOT NULL,
    email TEXT NOT NULL,
    addressLine1 TEXT NOT NULL,
    addressLine2 TEXT NOT NULL,
    country TEXT NOT NULL,
    city TEXT NOT NULL,
    zipCode TEXT NOT NULL,
    creditCardType TEXT NOT NULL,
    creditCardNumber TEXT NOT NULL,
    creditCardName TEXT NOT NULL,
    creditCardExpiryDate INTEGER NOT NULL,
    telephone TEXT NOT NULL,
    birthday INTEGER NOT NULL,
    gender TEXT NOT NULL,
    registrationDate INTEGER NOT NULL
);

CREATE TABLE Author
(
    id INTEGER NOT NULL PRIMARY KEY,
    name TEXT NOT NULL,
    bio TEXT NOT NULL
);

CREATE TABLE Book
(
    id INTEGER NOT NULL PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    unitPrice FLOAT NOT NULL,
    quantity INTEGER NOT NULL DEFAULT 0,
    soldQuantity INTEGER NOT NULL DEFAULT 0,
    views INTEGER NOT NULL DEFAULT 0,
    authorId INTEGER NOT NULL REFERENCES Author(id),
    minAge INTEGER NOT NULL,
    maxAge INTEGER NOT NULL,
    numberOfPages INTEGER NOT NULL,
    coverType TEXT NOT NULL,
    publisher TEXT NOT NULL,
    publicationLocation TEXT NOT NULL,
    publicationDate INTEGER NOT NULL,
    rating FLOAT NOT NULL DEFAULT 0,
    rating1 FLOAT NOT NULL DEFAULT 0,
    rating2 FLOAT NOT NULL DEFAULT 0,
    rating3 FLOAT NOT NULL DEFAULT 0,
    rating4 FLOAT NOT NULL DEFAULT 0,
    rating5 FLOAT NOT NULL DEFAULT 0,
    ratersCount INTEGER NOT NULL DEFAULT 0
);


CREATE TABLE Category
(
    id INTEGER NOT NULL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE BookCategory
(
    id INTEGER NOT NULL PRIMARY KEY,
    bookId INTEGER NOT NULL REFERENCES Book(id),
    CategoryId INTEGER NOT NULL REFERENCES Category(id)
);

CREATE TABLE CartItem
(
    id INTEGER NOT NULL PRIMARY KEY,
    customerId INTEGER NOT NULL REFERENCES Customer(id),
    bookId INTEGER NOT NULL REFERENCES Book(id),
    quantity INTEGER NOT NULL CHECK(quantity > 0)
);

CREATE TABLE Discount
(
    id INTEGER NOT NULL PRIMARY KEY,
    bookId INTEGER NOT NULL REFERENCES Book(id),
    unitPrice FLOAT NOT NULL,
    startTime INTEGER NOT NULL,
    endTime INTEGER NOT NULL,
    CHECK (startTime < endTime)
);

CREATE TABLE BOrder
(
    id INTEGER NOT NULL PRIMARY KEY,
    orderDate INTEGER NOT NULL,
    shippingAddress TEXT NOT NULL,
    total FLOAT NOT NULL,
    customerId INTEGER NOT NULL REFERENCES Customer(id)
);

CREATE TABLE BOrderItem
(
    id INTEGER NOT NULL PRIMARY KEY,
    bookId INTEGER NOT NULL REFERENCES Book(id),
    quantity INTEGER NOT NULL,
    unitPrice FLOAT NOT NULL,
    bOrderId INTEGER NOT NULL REFERENCES BOrder(id)
);

CREATE TABLE WishListItem
(
    id INTEGER NOT NULL PRIMARY KEY,
    bookId INTEGER NOT NULL REFERENCES Book(id),
    customerId INTEGER NOT NULL REFERENCES Customer(id)
);