using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Shared.Helpers
{
    public static class ValidationHelper
    {
        public static bool TryValidate(object ob, out List<ValidationResult> validationResults)
        {
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(ob, new ValidationContext(ob), results);

            validationResults = results;

            return isValid;
        }
    }
}
