using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPIBooks.App_Code;
using WebAPIBooks.Controllers;

namespace WebAPIBooksTests {
    [TestClass]
    public class RPNCalculatorUnitTests {
        [TestMethod]
        public void SingleOperand() {
            Assert.AreEqual(2, GetResult("2"));
            Assert.AreEqual(2.5, GetResult("2.5"));
            Assert.AreEqual(-3, GetResult("-3"));
        }
        [TestMethod]
        public void Sum() {
            Assert.AreEqual(2 + 3, GetResult("2 3 +"));
            Assert.AreEqual(2 - 3, GetResult("2 -3 +"));
            Assert.AreEqual(2.1 - 3, GetResult("2.1 -3 +"));
        }
        [TestMethod]
        public void Dec() {
            Assert.AreEqual(2 - 3, GetResult("2 3 -"));
            Assert.AreEqual(2 + 3, GetResult("2 -3 -"));
            Assert.AreEqual(2.1 + 3, GetResult("2.1 -3 -"));
        }
        [TestMethod]
        public void Mult() {
            Assert.AreEqual(2 * 3, GetResult("2 3 *"));
            Assert.AreEqual(2 * (-3), GetResult("2 -3 *"));
            Assert.AreEqual(2.1 * (-3), GetResult("2.1 -3 *"));
        }
        [TestMethod]
        public void Div() {
            Assert.AreEqual(2.0 / 3, GetResult("2 3 /"));
            Assert.AreEqual(2.0 / (-3), GetResult("2 -3 /"));
            Assert.AreEqual(2.1 / (-3), GetResult("2.1 -3 /"));
        }
        [TestMethod]
        public void Pow() {
            Assert.AreEqual(Math.Pow(2, 3), GetResult("2 3 ^"));
            Assert.AreEqual(Math.Pow(2, -3), GetResult("2 -3 ^"));
            Assert.AreEqual(Math.Pow(2.1, -3), GetResult("2.1 -3 ^"));
        }
        [TestMethod]
        public void TwoOperators() {
            Assert.AreEqual(2 + 3 + 5, GetResult("2 3 5 + +"));
            Assert.AreEqual(8, GetResult("10 4 2 / -"));
            Assert.AreEqual(4, GetResult("10 2 - 2 /"));
        }
        [TestMethod]
        public void IgnoreExSpaces() {
            Assert.AreEqual(2 + 3 + 5, GetResult(" 2  3   5    +   + "));
            Assert.AreEqual(8, GetResult("     10    4   2   /  -   "));
        }
        [TestMethod]
        public void InvalidCases() {
            AssertThrowsException("+");
            AssertThrowsException("-");
            AssertThrowsException("*");
            AssertThrowsException("/");
            AssertThrowsException("^");
            AssertThrowsException("+ +");
            AssertThrowsException("1 +");
            AssertThrowsException("+ 1");
            AssertThrowsException("1 2 - -");
            AssertThrowsException("1 2 - 4 - -");
        }

        static RPNController CreateController() {
            return new RPNController() {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
            };
        }
        static double GetResult(string expression) {
            return CreateController().Get(expression).GetMessage().GetContent<double>();
        }
        static void AssertThrowsException(string expression) {
            Assert.ThrowsException<AssertFailedException>(() => CreateController().Get(expression).GetMessage().AssertStatusCode());
        }
    }
}
