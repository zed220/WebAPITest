using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models {
    public class Book {
        public string Caption { get; set; }//req <=30
        public IList<Author> Authors => new List<Author>();//req >=1
        public int Pages { get; set; }//req >0; <=1000
        public int Year { get; set; }//opt <=1800
        public string Publisher { get; set; }//opt <=30
        public string ISBN { get; set; }//req, valid mask, valid net
    }
}