# WebAPITest

Usage:

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

Create new book (id must been unique):

<b>api/books</b>

Create new book's image by id=1:

<b>api/books?imageId=1</b>

<b>[PUT queries]</b>

Edit existing book by id=1:

<b>api/books/1</b>

Change existing book's image by id=1:

<b>api/books/ImageId=1</b>

<b>[DELETE queries]</b>

