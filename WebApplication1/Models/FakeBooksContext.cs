using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models {
    public class FakeBooksContext : IBooksContext {
        FakeBooksContext() {
            Books = new List<Book>();
        }

        public IList<Book> Books { get; }


        public static IBooksContext Create() {
            var context = new FakeBooksContext();
            context.Books.Add(new Book() {
                Id = 0,
                Caption = "Alice's Adventures in Wonderland",
                Authors = new List<Author>() {
                    new Author() { FirstName = "Lewis", LastName = "Carroll" }
                },
                ISBN = "0486275434",//978-0486275437
                Pages = 86,
                Publisher = "Dover Publications",
                Year = 1993
            });
            return context;
        }
    }

    public interface IBooksContext {
        IList<Book> Books { get; }
    }
}