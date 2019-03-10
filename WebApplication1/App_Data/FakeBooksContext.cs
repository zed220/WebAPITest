using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.App_Data {
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
                Title = "Alice in Wonderland",
                Authors = new[] {
                    new Author() { FirstName = "Lewis", LastName = "Carroll" }
                },
                ISBN = "0486275434",
                Pages = 86,
                Publisher = "Dover Publications",
                Year = 1993
            });
            context.Books.Add(new Book() {
                Id = 1,
                Title = "Adventures of Tom Sawyer",
                Authors = new[] {
                    new Author() { FirstName = "Mark", LastName = "Twain" }
                },
                ISBN = "9781948132824",
                Pages = 270,
                Publisher = "SeaWolf Press",
                Year = 2018
            });
            return context;
        }
    }

    public interface IBooksContext {
        int InvalidListIndex { get; }
        IList<Book> Books { get; }
    }
}