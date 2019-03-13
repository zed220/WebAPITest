using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProductCodeValidator;

namespace WebAPIBooks.App_Code {
    public static class Validator_ISBN {
        public static ValidationResult Check(object value, ValidationContext context) {
            string isbn = CorrectISBN(value.ToString());
            switch(isbn.Length) {
                case 10:
                    isbn = CorrectISBN10(isbn);
                    break;
                case 13:
                    isbn = CorrectISBN13(isbn);
                    break;
                default:
                    return new ValidationResult("ISBN has wrong length");
            }
            if(!IsbnValidator.IsValidIsbn($"ISBN {isbn}"))
                return new ValidationResult("ISBN has invalid format");
            return ValidationResult.Success;
        }
        public static string CorrectISBN(string original) {
            return original.ToString().Replace("-", String.Empty);
        }
        static string CorrectISBN10(string original) {
            return original.Insert(1, "-").Insert(7, "-").Insert(11, "-");
        }
        static string CorrectISBN13(string original) {
            return original.Insert(3, "-").Insert(5, "-").Insert(9, "-").Insert(15, "-");
        }
    }
}