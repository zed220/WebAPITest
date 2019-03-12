using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIBooks.App_Code {
    public static class Validator_ISBN {
        public static ValidationResult Check(object value, ValidationContext context) {
            return ValidationResult.Success;
        }
    }
}