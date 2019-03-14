using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIBooks.App_Code;

namespace WebAPIBooks.Controllers {
    public class RPNController : ApiController {
        // GET: api/RPN/5
        //" "=%20, "+"=%2B
        public IHttpActionResult Get(string expression) {
            var result = Calculator_RPN.Calculate(expression);
            return BadRequest();
            //return RPN_Calculator;
        }
    }
}
