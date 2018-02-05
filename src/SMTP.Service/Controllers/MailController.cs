using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SMTP.Service.Configuration;
using SMTP.Service.SMTPLibrary;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace SMTP.Service.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private ISmtpLibrary smtpLibrary;
        
        private ILogger<MailController> logger;
        private string smtpUser;
        private string smtpPassword;
        private string smtpHost;
        private int smtpPort;
        public MailController(ISmtpLibrary smtpLibrary, ILoggerFactory loggerFactory, 
            IOptions<SMTPServiceConfiguration> smptpConfiguration,
            IOptions<CloudFoundryServicesOptions> serviceOptions)
        {
            logger = loggerFactory.CreateLogger<MailController>();
            this.smtpLibrary = smtpLibrary;

            if (serviceOptions != null)
            {
                 
                var credentials = serviceOptions.Value.ServicesList.FirstOrDefault(services =>
                                                                 (services.Name.CompareTo(smptpConfiguration.Value.ServiceBindingName) == 0)).Credentials;
                if (credentials == null)
                    logger.LogCritical("SMTP Credential missing");
                else
                {
                    smtpHost = credentials["hostname"].Value;
                    smtpPort = int.Parse(credentials["port"].Value);
                    smtpUser = credentials["username"].Value;
                    smtpPassword = credentials["password"].Value;

                }
                

            }

            
            
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("The application is running");
        }

        
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        
        
    }
}
