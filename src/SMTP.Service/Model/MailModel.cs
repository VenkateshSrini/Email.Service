using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SMTP.Service.Model.CustomAttributes;

namespace SMTP.Service.Model
{
    public class MailModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="From Address cannot be empty")]
        [EmailAddress(ErrorMessage ="Invalid from address email")]
        public string FromAddress { get; set; }
        [CustomValidation(typeof(CustomValidations), "MandatoryEmailList",ErrorMessage ="To List cannot be empty")]
        [IsListOfEmailAddress(ErrorMessage ="One or some of the email provided in the To address list is invalid/Empty")]
        public List<string> ToAddress { get; set; }
        [IsListOfEmailAddress(ErrorMessage = "One or some of the email provided in the Bcc address list is invalid")]
        public List<string> Bcc { get; set; }
        [IsListOfEmailAddress(ErrorMessage = "One or some of the email provided in the cc address list is invalid")]
        public List<string> Cc { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Subject cannot be empty")]
        public string Subject { get; set; }

        public string  Body { get; set; }
        
    }
}
