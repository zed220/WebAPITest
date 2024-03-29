﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPIBooks.App_Data;
using WebAPIBooks.Controllers;
using WebAPIBooks.Models;

namespace WebAPIBooksTests {
    [TestClass]
    public class BooksControllerUnitTests {
        BooksController Controller;

        static Book CreateValidBook() =>
            new Book() {
                Id = 2,
                Title = "Chronicles of Amber",
                Authors = new[] {
                new Author() { FirstName = "Roger", LastName = "Zelazny" }
            },
                ISBN = "0380809060",
                Pages = 1264,
                Publisher = "Harper Voyager",
                Year = 2010
            };

        string GetImageFilePath(int imageId) {
            if(restoreImagesAction == null) {
                byte[] firstImage = File.ReadAllBytes(GetImagePathCore(0));
                byte[] secondImage = File.ReadAllBytes(GetImagePathCore(1));

                restoreImagesAction = new Action(() => {
                    File.WriteAllBytes(GetImagePathCore(0), firstImage);
                    File.WriteAllBytes(GetImagePathCore(1), secondImage);
                });
            }
            return GetImagePathCore(imageId);
        }
        static string GetImagePathCore(int imageId) {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), BooksController.ImageFolderName, $"{imageId}.png"));
        }

        Action restoreImagesAction;

        [TestInitialize]
        public void SetUp() {
            Controller = new BooksController() {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
                Test_GetImagePath = GetImageFilePath
            };
        }
        [TestCleanup]
        public void TearDown() {
            restoreImagesAction?.Invoke();
            restoreImagesAction = null;
            FakeBooksContext.Test_Clear();
            Controller.Dispose();
            Controller = null;
        }
        [TestMethod]
        public void GetDefaultBookById() {
            Action<int, Func<Book>> assertAction = (bookId, getOriginalBook) => {
                var message = Controller.GetBook(bookId).GetMessage();
                var book = message.GetContent<Book>();
                Assert.AreEqual(getOriginalBook(), book);
            };
            assertAction(0, () => FakeBooksContext.DefaultBook_0);
            assertAction(1, () => FakeBooksContext.DefaultBook_1);
        }
        [TestMethod]
        public void GetDefaultBooks_DefaultSortOrder() {
            Action<IHttpActionResult> assertSortByName = result => {
                var message = result.GetMessage();
                var books = message.GetContent<IEnumerable<Book>>().ToList();
                Assert.AreEqual(2, books.Count);
                Assert.AreEqual(FakeBooksContext.DefaultBook_1.Id, books[0].Id);
                Assert.AreEqual(FakeBooksContext.DefaultBook_0.Id, books[1].Id);
            };
            assertSortByName(Controller.GetBooks());
            assertSortByName(Controller.GetSortedBooks(SortMode.Title));
        }
        [TestMethod]
        public void GetDefaultBooks_SortByYear() {
            var message = Controller.GetSortedBooks(SortMode.Year).GetMessage();
            var books = message.GetContent<IEnumerable<Book>>().ToList();
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual(FakeBooksContext.DefaultBook_0.Id, books[0].Id);
            Assert.AreEqual(FakeBooksContext.DefaultBook_1.Id, books[1].Id);
        }
        [TestMethod]
        public void GetBookByInvalidId() {
            Action<int> assertAction = invalidId => {
                var message = Controller.GetBook(invalidId).GetMessage();
                Assert.ThrowsException<AssertFailedException>(message.GetContent<Book>);
            };
            assertAction(-1);
            assertAction(2);
        }
        [TestMethod]
        public void TryGetImageByInvalidSettings() {
            Action<int> assertAction = bookId => {
                var message = Controller.GetBookImage(bookId);
                Assert.ThrowsException<AssertFailedException>(message.GetByteArrayContent);
            };
            assertAction(-1);
            assertAction(2);
        }
        [TestMethod]
        public void GetImage() {
            Action<int, byte[]> assertAction = (bookId, fileArr) => {
                var message = Controller.GetBookImage(bookId);
                var byteArr = message.GetByteArrayContent();
                CollectionAssert.AreEqual(fileArr, byteArr);
            };
            assertAction(0, File.ReadAllBytes(GetImageFilePath(0)));
            assertAction(1, File.ReadAllBytes(GetImageFilePath(1)));
        }
        [TestMethod]
        public void AddWrongBook() {
            Action<int> assertAction = bookId => {
                var book = CreateValidBook();
                book.Id = bookId;
                Controller.ModelState.Clear();
                Controller.Validate(book);
                Assert.ThrowsException<AssertFailedException>(() => Controller.CreateBook(book).GetMessage().AssertStatusCode());
            };
            assertAction(-1);
            assertAction(0);
            assertAction(1);
        }
        [TestMethod]
        public void AddNewBook() {
            var book = CreateValidBook();
            Controller.ModelState.Clear();
            Controller.Validate(book);
            Controller.CreateBook(book).GetMessage().AssertStatusCode();
            var newBook = Controller.GetBook(2).GetMessage().GetContent<Book>();
            Assert.IsNotNull(newBook);
            Assert.AreEqual(book, newBook);
            Assert.AreEqual(3, Controller.GetBooks().GetMessage().GetContent<IEnumerable<Book>>().ToList().Count);
        }
        [TestMethod]
        public void AddNewBook_FixISBN() {
            var book = CreateValidBook();
            book.ISBN = "-0---3-8-0-8--0-9-0-6-0--";
            Controller.ModelState.Clear();
            Controller.Validate(book);
            Controller.CreateBook(book).GetMessage().AssertStatusCode();
            var newBook = Controller.GetBook(2).GetMessage().GetContent<Book>();
            Assert.IsNotNull(newBook);
            Assert.AreEqual(book, newBook);
            Assert.AreEqual(newBook.ISBN.Replace("-", ""), newBook.ISBN);
            Assert.AreEqual(3, Controller.GetBooks().GetMessage().GetContent<IEnumerable<Book>>().ToList().Count);
        }

        void AssertInvalidBookAllCases(Action<Book> controllerAction) {
            Action<Action<Book>> assertAction = broke => {
                var book = CreateValidBook();
                broke(book);
                Controller.ModelState.Clear();
                Controller.Validate(book);
                Assert.ThrowsException<AssertFailedException>(() => controllerAction(book));
            };
            assertAction(b => { b.Id = -1; });
            assertAction(b => { b.Title = null; });
            assertAction(b => { b.Title = new string('c', 31); });
            assertAction(b => { b.Pages = 0; });
            assertAction(b => { b.Pages = 10001; });
            assertAction(b => { b.Year = 1799; });
            assertAction(b => { b.Year = DateTime.Now.Year + 1; });
            assertAction(b => { b.Publisher = new string('c', 31); });
            assertAction(b => { b.Authors = null; });
            assertAction(b => { b.Authors = new Author[0]; });
            assertAction(b => { b.Authors[0].FirstName = null; });
            assertAction(b => { b.Authors[0].FirstName = new string('c', 21); });
            assertAction(b => { b.Authors[0].LastName = null; });
            assertAction(b => { b.Authors[0].LastName = new string('c', 21); });
            assertAction(b => { b.ISBN += "1"; });
            assertAction(b => { b.ISBN = b.ISBN.Replace("0", "a"); });
        }
        [TestMethod]
        public void TryAddInvalidBook() {
            AssertInvalidBookAllCases(book => Controller.CreateBook(book).GetMessage().AssertStatusCode());
        }
        [TestMethod]
        public void TryModifyInvalidBook() {
            AssertInvalidBookAllCases(book => Controller.EditBook(-1, book).GetMessage().AssertStatusCode());
            AssertInvalidBookAllCases(book => {
                if(book.Id != -1)
                    book.Id = 0;
                Controller.ModelState.Clear();
                Controller.Validate(book);
                Controller.EditBook(0, book).GetMessage().AssertStatusCode();
            });
        }
        [TestMethod]
        public void ModifyBook() {
            Action<string> modifyAction = isbn => {
                var book = CreateValidBook();
                book.Id = 0;
                book.ISBN = isbn;
                Controller.ModelState.Clear();
                Controller.Validate(book);
                Controller.EditBook(0, book).GetMessage().AssertStatusCode();
                var newBook = Controller.GetBook(0).GetMessage().GetContent<Book>();
                Assert.AreEqual(book, newBook);
                Assert.AreEqual(2, Controller.GetBooks().GetMessage().GetContent<IEnumerable<Book>>().ToList().Count);
                Assert.AreEqual(isbn.Replace("-", ""), newBook.ISBN);
            };
            modifyAction("0380809060");
            modifyAction("0380809060-");
            modifyAction("-0380809060-");
            modifyAction("-0-3-8-0-8---0-9-0-6-0-");
            modifyAction("9781948132824");
            modifyAction("9781948132824-");
            modifyAction("-9781948132824-");
            modifyAction("-9--7-8--1-9---4--8-1-3-2--8-24-");
        }
        [TestMethod]
        public void TryDeleteInvalidBook() {
            Action<int> deleteAction = id => {
                Assert.ThrowsException<AssertFailedException>(() => Controller.DeleteBook(id).GetMessage().AssertStatusCode());
            };
            deleteAction(-1);
            deleteAction(3);
        }
        [TestMethod]
        public void DeleteBook() {
            var path = GetImageFilePath(0);
            Assert.IsTrue(File.Exists(path));
            Controller.DeleteBook(0).GetMessage().AssertStatusCode();
            Assert.IsFalse(File.Exists(path));
            var books = Controller.GetBooks().GetMessage().GetContent<IEnumerable<Book>>().ToList();
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual(FakeBooksContext.DefaultBook_1, books.Single());
        }
        //add new image
        //modify existing image
        //try add invalid image
        //tests with real browser
    }
}
