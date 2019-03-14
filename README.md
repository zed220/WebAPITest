# WebAPITest

Books API Usage:

<b>[GET queries]</b>

Get all books with default sorting by Title:

<b>api/books</b>

Get all books with sorting by Title:

<b>api/books?sortMode=Title</b>

Get all books with sorting by publish year:

<b>api/books?sortMode=Year</b>

Get book by id=1:

<b>api/books/1</b>

Get book image by id=1:

<b>api/books?imageId=1</b>

<b>[POST queries]</b>

Create new book (id must be unique):

<b>api/books</b>

Create new or replace old book's image by id=1:

<b>api/books?imageId=1</b>

<b>[PUT queries]</b>

Edit existing book by id=1:

<b>api/books/1</b>

<b>[DELETE queries]</b>

Delete book by id=1:

<b>api/books/1</b>

RPN API Usage:

<b>[GET queries]</b>

Get result by string expression "1 2 +" (use %20 for " " and %2B for "+" symbols):

<b>api/rpn?expression=1%202%20%2B</b>
