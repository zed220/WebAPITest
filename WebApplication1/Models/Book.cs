using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAPIBooks.App_Code;

namespace WebAPIBooks.Models {
    public class Book {
        [Range(0, int.MaxValue)]
        public int Id { get; set; } = -1;
        [Required, StringLength(30)]
        public string Title { get; set; }
        [Required, MinLength(1)]
        public Author[] Authors { get; set; }
        [Required, Range(0, 10000)]
        public int Pages { get; set; }
        [Range(1800, 2019)]
        public int Year { get; set; }
        [StringLength(30)]
        public string Publisher { get; set; }
        [CustomValidation(typeof(Validator_ISBN), nameof(Validator_ISBN.Check))]
        public string ISBN { get; set; }//req, valid mask, valid net
    }
}