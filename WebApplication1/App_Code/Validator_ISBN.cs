using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProductCodeValidator;

namespace WebApplication1.App_Code {
    public static class Validator_ISBN {
        public static ValidationResult Check(object value, ValidationContext context) {
            string isbn = value.ToString();
            if(IsbnValidator.EanIsIsbn(isbn))
                return ValidationResult.Success;
            
            //ProductCodeValidator.IsbnValidator.
            return ValidationResult.Success;
        }
    }
}