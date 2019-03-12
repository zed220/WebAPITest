using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPIBooks.App_Data;
using WebAPIBooks.Controllers;
using WebAPIBooks.Models;

namespace WebAPIBooksTests {
    [TestClass]
    public class BooksControllerUnitTests {
        BooksController Controller;

        static string GetImageFilePath(int imageId) => Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), BooksController.ImageFolderName, $"{imageId}.png"));

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
            FakeBooksContext.Test_Clear();
            Controller.Dispose();
            Controller = null;
        }

        HttpResponseMessage GetMessage(IHttpActionResult result) => result.ExecuteAsync(new CancellationToken()).Result;

        [TestMethod]
        public void GetDefaultBookById() {
            Action<int, Func<Book>> assertAction = (bookId, getOriginalBook) => {
                var message = GetMessage(Controller.GetBook(bookId));
                var book = message.GetContent<Book>();
                Assert.AreEqual(getOriginalBook(), book);
            };
            assertAction(0, () => FakeBooksContext.DefaultBook_0);
            assertAction(1, () => FakeBooksContext.DefaultBook_1);
        }
        [TestMethod]
        public void GetDefaultBooks_DefaultSortOrder() {
            Action<IHttpActionResult> assertSortByName = result => {
                var message = GetMessage(result);
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
            var message = GetMessage(Controller.GetSortedBooks(SortMode.Year));
            var books = message.GetContent<IEnumerable<Book>>().ToList();
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual(FakeBooksContext.DefaultBook_0.Id, books[0].Id);
            Assert.AreEqual(FakeBooksContext.DefaultBook_1.Id, books[1].Id);
        }

        [TestMethod]
        public void GetBookByInvalidId() {
            Action<int> assertAction = invalidId => {
                var message = GetMessage(Controller.GetBook(invalidId));
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

        //add new book
        //modify existion book
        //add image
        //modify image
    }

    public static class BooksControllerExtensions {
        public static T GetContent<T>(this HttpResponseMessage message) {
            if(!message.IsSuccessStatusCode)
                Assert.Fail($"Bad Request. Code={message.StatusCode}, Text={message.Content?.ReadAsAsync<HttpError>().Result.Message}");
            return message.Content.ReadAsAsync<T>().Result;
        }
        public static byte[] GetByteArrayContent(this HttpResponseMessage message) {
            if(!message.IsSuccessStatusCode)
                Assert.Fail($"Bad Request. Code={message.StatusCode}, Text={message.Content?.ReadAsAsync<HttpError>().Result.Message}");
            return message.Content.ReadAsByteArrayAsync().Result;
        }
    }
}
