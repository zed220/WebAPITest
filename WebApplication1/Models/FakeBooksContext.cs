using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models {
    public class FakeBooksContext : IBooksContext {
        FakeBooksContext() {
            Books = new List<Book>();
        }

        public int InvalidListIndex => -1;
        public IList<Book> Books { get; }

        static IBooksContext instance;
        public static IBooksContext Instance {
            get {
                if(instance == null)
                    instance = CreateWithFakeValues();
                return instance;
            }
        }

        static IBooksContext CreateWithFakeValues() {
            var context = new FakeBooksContext();
            context.Books.Add(new Book() {
                Id = 0,
                Caption = "Alice's Adventures in Wonderland",
                Authors = new[] {
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
        int InvalidListIndex { get; }
        IList<Book> Books { get; }
    }
}