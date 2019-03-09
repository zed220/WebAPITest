using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models {
    public class Book {

        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Caption { get; set; }
        [Required, MinLength(1)]
        public Author[] Authors { get; set; }
        [Required, Range(0, 10000)]
        public int Pages { get; set; }
        [Range(1800, 2019)]
        public int Year { get; set; }
        [StringLength(30)]
        public string Publisher { get; set; }
        public string ISBN { get; set; }//req, valid mask, valid net
    }
}