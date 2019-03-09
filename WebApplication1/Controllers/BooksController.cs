using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    public class BooksController : ApiController {
        const int InvalidListIndex = -1;

        IBooksContext booksContext = FakeBooksContext.Create();

        // GET api/books
        public IEnumerable<Book> GetBooks() {
            return booksContext.Books;
        }

        // GET api/books/5
        public IHttpActionResult GetBook(int id) {
            var book = GetBookCore(id);
            if(book == null)
                return BadRequest();
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IHttpActionResult CreateBook([FromBody]Book book) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            booksContext.Books.Add(book);
            return Ok();
        }

        // PUT api/books/5
        [HttpPut]
        public void EditBook(int id, [FromBody]Book book) {
            if(id != book.Id)
                return;
            var listIndex = GetBookListIndex(id);
            if(listIndex == InvalidListIndex)
                return;
            booksContext.Books[listIndex] = book; 
        }

        // DELETE api/books/5
        public void DeleteBook(int id) {
            var listIndex = GetBookListIndex(id);
            if(listIndex == InvalidListIndex)
                return;
            booksContext.Books.RemoveAt(listIndex);
        }

        Book GetBookCore(int id) {
            return booksContext.Books.SingleOrDefault(b => b.Id == id);
        }
        int GetBookListIndex(int id) {
            var book = GetBookCore(id);
            if(book == null)
                return InvalidListIndex;
            return booksContext.Books.IndexOf(book);
        }
    }
}
