using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMTP.Service.Test
{
    public static class Utilities
    {
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
