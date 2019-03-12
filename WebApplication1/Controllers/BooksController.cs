using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebApplication1.App_Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    public class BooksController : ApiController {
        IBooksContext booksContext = FakeBooksContext.Instance;

        // GET api/books
        public IHttpActionResult GetBooks() {
            return GetSortedBooks(SortMode.Title);
        }

        //Get api/books/?sortMode
        public IHttpActionResult GetSortedBooks([FromUri]SortMode sortMode) {
            switch(sortMode) {
                case SortMode.Title: return Ok(booksContext.Books.OrderBy(x => x.Title));
                case SortMode.Year: return Ok(booksContext.Books.OrderBy(x => x.Year));
                default: return BadRequest();
            }
        }

        //Get api/books/?image
        public HttpResponseMessage GetBookImage([FromUri]int imageId) {
            if(GetBookCore(imageId) == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            var path = System.Web.Hosting.HostingEnvironment.MapPath($@"~\Bin\StoreImages\{imageId}.png");
            if(!File.Exists(path))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(File.ReadAllBytes(path));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }

        // GET api/books/id
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
