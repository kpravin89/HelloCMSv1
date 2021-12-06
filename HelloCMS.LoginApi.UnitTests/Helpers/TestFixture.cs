using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelloCMS.Identity.UnitTests.Helpers
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {

        public readonly TestServer Server;
        private readonly HttpClient _client;


        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot($"..\\Services\\Identity\\HelloCMS.Identity\\")
                .UseStartup<TStartup>();

            Server = new TestServer(builder);
        }


        public void Dispose()
        {
            _client.Dispose();
            Server.Dispose();
        }
    }
}
