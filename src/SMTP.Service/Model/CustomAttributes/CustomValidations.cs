using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Service.Model.CustomAttributes
{
    public class CustomValidations
    {
        public static ValidationResult MandatoryEmailList(List<string> values)
        {
            if (values == null) return new ValidationResult("List of email address cannt be null");
            if (values.Count==0) return new ValidationResult("List of email address cannt be empty");
            return ValidationResult.Success;
        }
    }
}
