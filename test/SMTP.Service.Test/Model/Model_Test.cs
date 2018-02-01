using SMTP.Service.Model;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;
namespace SMTP.Service.Test.Model
{
    public class Model_Test
    {
        [Fact]
        public void RequiredValidationTest()
        {
            MailModel mailModel = new MailModel();
            List<ValidationResult> results = new List<ValidationResult>(Utilities.ValidateModel(mailModel));
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("From Address cannot be empty") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("List of email address cannt be null") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("Subject cannot be empty") == 0);
        }
        [Fact]
        public void EmptyValidationTest()
        {
            MailModel mailModel = new MailModel()
            {
                FromAddress = string.Empty,
                ToAddress = new List<string>(),
                Subject = string.Empty
            };
            List<ValidationResult> results = new List<ValidationResult>(Utilities.ValidateModel(mailModel));
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("From Address cannot be empty") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("List of email address cannt be empty") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("Subject cannot be empty") == 0);
        }
        [Fact]
        public void InvalidEmailAddressTest()
        {
            MailModel mailModel = new MailModel()
            {
                FromAddress = "abc",
                ToAddress = new List<string>() { "abc"},
                Bcc = new List<string>() { "abc" },
                Cc = new List<string>() { "abc" },
                Subject = "Test Subject"
            };
            List<ValidationResult> results = new List<ValidationResult>(Utilities.ValidateModel(mailModel));
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("Invalid from address email") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("One or some of the email provided in the To address list is invalid/Empty") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("One or some of the email provided in the Bcc address list is invalid") == 0);
            Assert.Contains(results, entry => entry.ErrorMessage.CompareTo("One or some of the email provided in the cc address list is invalid") == 0);


        }
    
    }
}
