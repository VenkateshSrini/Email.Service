using SMTP.Service.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using System.Net;

namespace SMTP.Service.Integration.Test.Controllers
{
    public class MailController_Test:TestBase
    {
        private HttpClient httpClient;
        public MailController_Test()
        {
            httpClient = testServer.CreateClient();
        }
        [Fact]
        public async void TestPost()
        {
            MailModel mailModel = new MailModel
            {
                FromAddress="svenkatesh_in@yahoo.com",
                ToAddress = new List<string>
                {
                    "venkateshsrini3@gmail.com",
                    "heman_1978@hotmail.com"
                },
                Subject = "Hi there",
                Body ="<html><head>issue</head></html>"
            };

            var response = await httpClient.PostAsync("/api/Mail", new StringContent(JsonConvert.SerializeObject(mailModel),
                                                                System.Text.Encoding.UTF8,
                                                                "application/json"
                                                                )
                                 );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
