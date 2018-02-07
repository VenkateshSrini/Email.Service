using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using SMTP.Service.Configuration;
using Microsoft.Extensions.Logging;
using SMTP.Service.SMTPLibrary;
using SMTP.Service.Model;
using System.Threading.Tasks;
using SMTP.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SMTP.Service.Test.Controllers
{
    public class MailController_Test
    {
        IOptions<CloudFoundryServicesOptions> cfOptions;
        IOptions<SMTPServiceConfiguration> smptOptions;
        ILoggerFactory loggerFactory;
        Mock<ISmtpLibrary> mockSMPTPLib;
        public MailController_Test()
        {
            IConfiguration configuration = CreateConfiguration();
            CloudFoundryServicesOptions cfServiceOptions = new CloudFoundryServicesOptions(configuration);
           
            cfOptions = Options.Create<CloudFoundryServicesOptions>(cfServiceOptions);
            SMTPServiceConfiguration smtpConfig = new SMTPServiceConfiguration
            {
                ServiceBindingName = "smtp-service"
            };
            smptOptions = Options.Create<SMTPServiceConfiguration>(smtpConfig);
            loggerFactory = new LoggerFactory();
            mockSMPTPLib = new Mock<ISmtpLibrary>();
            mockSMPTPLib.Setup(method => method.Send(It.IsAny<MailModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>()));
            mockSMPTPLib.Setup(method => method.SendAsync(It.IsAny<MailModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
               It.IsAny<int>())).Returns(Task.FromResult(0)).Verifiable();
        }
        
        private IConfiguration CreateConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true, true);
            return configurationBuilder.Build();
        }
        [Fact]
        public  void PostInvalid_Test()
        {
            MailController mailController = new MailController(mockSMPTPLib.Object, loggerFactory,
                smptOptions, cfOptions);
            MailModel mailModel = new MailModel()
            {
                FromAddress = "abc",
                ToAddress = new List<string>() { "abc" },
                Bcc = new List<string>() { "abc" },
                Cc = new List<string>() { "abc" },
                Subject = "Test Subject"
            };
            mailController.ModelState.AddModelError("From", "Invalid from address email");
            
            var actionResult = ( mailController.Post(mailModel)) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);
            var apiResponse = actionResult.Value as APIResponse;
            Assert.True(apiResponse.ModelState.Count > 0);

            
        }
        [Fact]
        public  void PostValid_Test()
        {
            MailController mailController = new MailController(mockSMPTPLib.Object, loggerFactory,
                smptOptions, cfOptions);
            MailModel mailModel = new MailModel()
            {
                FromAddress = "abc",
                ToAddress = new List<string>() { "abc" },
                Bcc = new List<string>() { "abc" },
                Cc = new List<string>() { "abc" },
                Subject = "Test Subject"
            };
            

            var actionResult = ( mailController.Post(mailModel)) as OkObjectResult;
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);
            var apiResponse = actionResult.Value as APIResponse;
            Assert.Equal(HttpStatusCode.OK, apiResponse.StatusCode);


        }

    }
   
}
