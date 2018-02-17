using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SMTP.Service.Configuration;
using SMTP.Service.Model;
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

            if ((serviceOptions != null) & (serviceOptions.Value.ServicesList.Count>0))
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
        public IActionResult Post([FromBody]MailModel mailModel)
        {
            string corelationID = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    logger.LogTrace($"corelation ID :{corelationID} model is valid. About to send email");
                    smtpLibrary.Send(mailModel, smtpHost, smtpUser, smtpPassword, smtpPort);
                    logger.LogTrace($"corelation ID :{corelationID} Email sent successfully");
                    return Ok(new APIResponse {
                        CorelationID = corelationID,
                        Status = "Mail sent sucessfully",
                        StatusCode = HttpStatusCode.OK
                    });
                }
                catch (AggregateException ex)
                {
                    logger.LogError($"corelation id : {corelationID}{ex.Message}", ex.StackTrace);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new APIResponse
                    {
                        CorelationID = corelationID,
                        Status = "Error while seding mail",
                        StatusCode = HttpStatusCode.InternalServerError,

                    });
                    
                }
            }
            else
            {
                logger.LogError($"corelation id : {corelationID} Validation error");
                return BadRequest(new APIResponse
                {
                    CorelationID = corelationID,
                    Status = "Error while seding mail",
                    StatusCode = HttpStatusCode.InternalServerError,
                    ModelState = ModelState
                });
            }

        }



    }
}
