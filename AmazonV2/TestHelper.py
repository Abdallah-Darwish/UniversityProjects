"""
Creates a new database and seeds it.
It will contain multiple accounts with usernames: username1 username2 ....
It must be placed in the same directory containing "Database" and "Images" directories
"""
from pathlib import Path
import sqlite3
import datetime
import random

backend_path = Path(__file__).parent
db_path = backend_path.joinpath('Database').joinpath('Db.sqlite')
images_path = backend_path.joinpath('Images')

def get_connection() -> sqlite3.Connection:
    global db_path
    c = sqlite3.connect(db_path)
    c.row_factory = sqlite3.Row
    c.execute('PRAGMA foreign_keys = ON')
    return c

if db_path.exists():
    db_path.unlink()

with open(str(backend_path.joinpath('Database').joinpath('DbInit.sql')), 'r') as init_scrip_fs:
    with get_connection() as con:
        con.executescript(init_scrip_fs.read())

def create_book_image(id: int, title: str) -> None:
    from PIL import Image, ImageDraw, ImageFont

    # create an image
    with Image.new("RGB", (150, 150), '#8eb5ef') as book_image:

        # get a font
        fnt = ImageFont.truetype("arial", 40)
        # get a drawing context
        d = ImageDraw.Draw(book_image)

        # draw multiline text
        d.multiline_text((10,10), '\n'.join(title.split()), font=fnt, fill=(0, 0, 0))

        book_image_path = images_path.joinpath(f'book{id}.jpg')
        book_image.save(str(book_image_path))
customers = []
def fill_customer() -> None:
    import base64
    with get_connection() as con:
        sql = '''INSERT INTO Customer
(
id,
firstName,
lastName,
userName,
passwordHash,
email,
addressLine1,
addressLine2,
country,
city,
zipCode,
creditCardType,
creditCardNumber,
creditCardName,
creditCardExpiryDate,
telephone,
birthday,
gender,
registrationDate)
VALUES
(:id,
:firstName,
:lastName,
:userName,
:passwordHash,
:email,
:addressLine1,
:addressLine2,
:country,
:city,
:zipCode,
:creditCardType,
:creditCardNumber,
:creditCardName,
:creditCardExpiryDate,
:telephone,
:birthday,
:gender,
:registrationDate
)'''
        password_hash = base64.b64encode(b'123456789')
        global customers
        for i in range(1, 30):
            customers.append(i)
            con.execute(sql, 
            {
            'id' : i,
            'firstName' : f'{i} first name',
            'lastName' : f'{i} last name',
            'userName' : f'username{i}',
            'passwordHash' : password_hash,
            'email' : f'email{i}@email{i}.com',
            'addressLine1' : f'user {i} address line 1',
            'addressLine2' : f'user {i} address line 2',
            'country' : f'user {i} country',
            'city' : f'user {i} city',
            'zipCode' : '1234',
            'creditCardType' : f'user {i} credit card type',
            'creditCardNumber' : '1234-1234-1234-1234-1234-1234',
            'creditCardName' : f'{i} first name {i} last name',
            'creditCardExpiryDate' : f'{i} first name {i} last name',
            'telephone' : '123456789',
            'birthday' : int(datetime.datetime(random.randint(2000, 2005), random.randint(1, 12), random.randint(1, 20)).timestamp()),
            'gender' : 'male',
            'registrationDate' : int(datetime.datetime(random.randint(2010, 2021), random.randint(1, 12), random.randint(1, 20)).timestamp())
            })
authors = []
def fill_author() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO Author
(
id,
name,
bio
)
VALUES
(
:id,
:name,
:bio
)'''
        global authors
        for i in range(1, 20):
            authors.append(i)
            con.execute(sql, 
            {
            'id' : i,
            'name' : f'Author {i}',
            'bio' : f'Author {i} bio' 
            })
categories = []
def fill_category() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO Category
(
id,
name
)
VALUES
(
:id,
:name
)'''
        global categories
        for i in range(1, 50):
            categories.append(i)
            con.execute(sql, 
            {
            'id' : i,
            'name' : f'Category {i}'
            })

books = []
def fill_book() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO Book
(
id,
name,
description,
unitPrice,
quantity,
soldQuantity,
views,
authorId,
minAge,
maxAge,
numberOfPages,
coverType,
publisher,
publicationLocation,
publicationDate,
rating,
rating1,
rating2,
rating3,
rating4,
rating5,
ratersCount
)
VALUES
(
:id,
:name,
:description,
:unitPrice,
:quantity,
:soldQuantity,
:views,
:authorId,
:minAge,
:maxAge,
:numberOfPages,
:coverType,
:publisher,
:publicationLocation,
:publicationDate,
:rating,
:rating1,
:rating2,
:rating3,
:rating4,
:rating5,
:ratersCount
)'''
        global authors
        for i in range(1, 200):
            books.append(i)
            con.execute(sql, 
            {
            'id' : i,
            'name' : f'Book {i}',
            'description' : f'Book {i} description',
            'unitPrice' : float(random.randint(1, 200)),
            'quantity' : random.randint(0, 1000),
            'soldQuantity' : random.randint(0, 5000),
            'views' : random.randint(0, 5000),
            'authorId' : random.choice(authors),
            'minAge' : random.randint(5, 20),
            'maxAge' : random.randint(25, 100),
            'numberOfPages' : random.randint(25, 100),
            'coverType' : f'Cover {i}',
            'publisher' : f'Publisher {i}',
            'publicationLocation' : f'City {i}',
            'publicationDate' : int(datetime.datetime(random.randint(1990, 2010), random.randint(1, 12), random.randint(1, 25)).timestamp()),
            'rating' : random.randint(0, 5),
            'rating1' : random.random(),
            'rating2' : random.random(),
            'rating3' : random.random(),
            'rating4' : random.random(),
            'rating5' : random.random(),
            'ratersCount' : random.randint(10, 10000)
            })
            create_book_image(i, f'Book {i}')

def fill_book_category() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO BookCategory
(
id,
bookId,
CategoryId
)
VALUES
(
:id,
:bookId,
:CategoryId
)'''
        global books
        global categories
        i = 1
        for b in books:
            cats = random.choices(categories, k=random.randint(1, len(categories)))
            for c in cats:
                con.execute(sql, 
                {
                'id' : i,
                'bookId' : b,
                'CategoryId' : c 
                })
                i += 1
def fill_cart_item() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO CartItem 
(
id,
customerId,
bookId,
quantity
)
VALUES
(
:id,
:customerId,
:bookId,
:quantity
)'''
        global books, customers
        i = 1
        for c in customers:
            customer_books = random.choices(books, k=random.randint(0, len(books)))
            for b in customer_books:
                con.execute(sql, 
                {
                'id' : i,
                'customerId' : c,
                'bookId' : b,
                'quantity' : random.randint(1, 10)
                })
                i += 1
def fill_wish_list_item() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO WishListItem
(
id,
bookId,
customerId
)
VALUES
(
:id,
:bookId,
:customerId
)'''
        global books, customers
        i = 1
        for c in customers:
            customer_books = random.choices(books, k=random.randint(0, len(books)))
            for b in customer_books:
                con.execute(sql, 
                {
                'id' : i,
                'bookId' : b,
                'customerId' : c
                })
                i += 1

bOrders = []
def fill_Border() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO BOrder
(
id,
orderDate,
shippingAddress,
total,
customerId
)
VALUES
(
:id,
:orderDate,
:shippingAddress,
:total,
:customerId
)'''
        global customers, bOrders
        id = 1
        for c in customers:
            for i in range(random.randint(1, 10)):
                con.execute(sql, 
                {
                'id' : id,
                'orderDate' : int(datetime.datetime(random.randint(2018, 2021), random.randint(1, 12), random.randint(1, 27)).timestamp()),
                'shippingAddress' : f'Order {id} Shipping address',
                'total' : float(random.randint(10, 1000)),
                'customerId' : c
                })
                bOrders.append(id)
                id += 1

def fill_border_item() -> None:
    with get_connection() as con:
        sql = '''INSERT INTO BOrderItem
(
id,
bookId,
quantity,
unitPrice,
bOrderId
)
VALUES
(
:id,
:bookId,
:quantity,
:unitPrice,
:bOrderId
)'''
        global bOrders, books
        i = 1
        for o in bOrders:
            cb = random.choices(books, k=random.randint(1, len(books)))
            for b in cb:
                con.execute(sql, 
                {
                'id' : i,
                'bookId' : b,
                'quantity' : random.randint(1, 10),
                'unitPrice' : random.randint(1, 100),
                'bOrderId' : o
                })
                i += 1
fill_customer()
fill_category()
fill_author()
fill_book()
fill_book_category()
fill_cart_item()
fill_Border()
fill_border_item()
fill_wish_list_item()