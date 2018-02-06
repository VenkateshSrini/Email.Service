using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using SMTP.Service;
namespace SMTP.Service.Integration.Test
{
    public class TestStartUp:Startup
    {
        public TestStartUp(IConfiguration configuration) : base(configuration) { }
    }
}
