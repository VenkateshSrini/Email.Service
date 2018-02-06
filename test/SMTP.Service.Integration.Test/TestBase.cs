using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP.Service.Integration.Test
{
   public abstract class TestBase:IDisposable
    {
        protected TestServer testServer;
        public static IConfiguration GetConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                       .AddJsonFile("appSettings.json", true)
                       .AddCommandLine(args)
                       .Build();
        }

        public void Dispose()
        {
            testServer.Dispose();
        }

        public TestBase()
        {
            testServer = new TestServer(new WebHostBuilder()
                                             .UseConfiguration(GetConfiguration(new string[] { }))
                                             .UseStartup<TestStartUp>()
                                             .UseEnvironment("Development")
                                             .UseCloudFoundryHosting()
                                             .AddCloudFoundry());
        }


    }
}
