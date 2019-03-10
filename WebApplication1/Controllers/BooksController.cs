using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.App_Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    public class BooksController : ApiController {
        IBooksContext booksContext = FakeBooksContext.Instance;

        // GET api/books
        public IEnumerable<Book> GetBooks() {
            return booksContext.Books;
        }

        //Get api/books/?sorting
        [HttpGet]
        public IHttpActionResult GetSortedBooks([FromUri]SortMode sortMode) {
            switch(sortMode) {
                case SortMode.Title: return Ok(booksContext.Books.OrderBy(x => x.Title));
                case SortMode.Year: return Ok(booksContext.Books.OrderBy(x => x.Year));
                default: return BadRequest();
            }
        }

        // GET api/books/5
        public IHttpActionResult GetBook(int id) {
            var book = GetBookCore(id);
            if(book == null)
                return BadRequest(GetNotFoundErrorText(id));
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IHttpActionResult CreateBook([FromBody]Book book) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(GetBookListIndex(book.Id) != booksContext.InvalidListIndex)
                return BadRequest($"Book with Id={book.Id} already created.");
            booksContext.Books.Add(book);

            return Ok();
        }

        // PUT api/books/5
        [HttpPut]
        public IHttpActionResult EditBook(int id, [FromBody]Book book) {
            if(id != book.Id)
                return BadRequest($"Target Id={id} does not equal book's Id={book.Id}.");
            var listIndex = GetBookListIndex(id);
            if(listIndex == booksContext.InvalidListIndex)
                return BadRequest(GetNotFoundErrorText(id));
            booksContext.Books[listIndex] = book;
            return Ok();
        }

        // DELETE api/books/5
        public IHttpActionResult DeleteBook(int id) {
            var listIndex = GetBookListIndex(id);
            if(listIndex == booksContext.InvalidListIndex)
                return BadRequest(GetNotFoundErrorText(id));
            booksContext.Books.RemoveAt(listIndex);
            return Ok();
        }

        Book GetBookCore(int id) {
            return booksContext.Books.SingleOrDefault(b => b.Id == id);
        }
        int GetBookListIndex(int id) {
            var book = GetBookCore(id);
            if(book == null)
                return booksContext.InvalidListIndex;
            return booksContext.Books.IndexOf(book);
        }

        static string GetNotFoundErrorText(int id) => $"Book with Id={id} not found.";
    }

    public enum SortMode { Title, Year }
}
