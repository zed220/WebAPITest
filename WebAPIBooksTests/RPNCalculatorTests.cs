using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBooks.App_Code;
using WebAPIBooks.Controllers;

namespace WebAPIBooksTests {
    [TestClass]
    public class RPNCalculatorTests {
        [TestMethod]
        public void SimpleOperators() {
            Assert.AreEqual(3, new RPNController().Get("1 2 +").GetMessage().GetContent<double>());
        }
    }
}
