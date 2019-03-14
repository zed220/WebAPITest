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
    public class RPNCalculatorTests {
        [TestMethod]
        public void SimpleOperators() {
            Assert.AreEqual(2, GetResult("2"));
            Assert.AreEqual(2.5, GetResult("2.5"));
            Assert.AreEqual(-3, GetResult("-3"));
        }

        static double GetResult(string expression) {
            return new RPNController() {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
            }.Get(expression).GetMessage().GetContent<double>();
        }
    }
}
