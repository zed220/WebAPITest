using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    public class ValuesController : ApiController {
        const int InvalidListIndex = -1;

        IBooksContext booksContext = FakeBooksContext.Create();

        // GET api/values
        public IEnumerable<Book> GetBooks() {
            return booksContext.Books;
        }

        // GET api/values/5
        public Book GetBook(int id) {
            return booksContext.Books.SingleOrDefault(b => b.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void CreateBook([FromBody]Book book) {
            booksContext.Books.Add(book);
        }

        // PUT api/values/5
        [HttpPut]
        public void EditBuuk(int id, [FromBody]Book book) {
            if(id != book.Id)
                return;
            var listIndex = GetBookListIndex(id);
            if(listIndex == InvalidListIndex)
                return;
            booksContext.Books[listIndex] = book; 
        }

        // DELETE api/values/5
        public void DeleteBook(int id) {
            var listIndex = GetBookListIndex(id);
            if(listIndex == InvalidListIndex)
                return;
            booksContext.Books.RemoveAt(listIndex);
        }

        int GetBookListIndex(int id) {
            var book = GetBook(id);
            if(book == null)
                return InvalidListIndex;
            return booksContext.Books.IndexOf(book);
        }
    }
}
