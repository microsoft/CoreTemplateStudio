using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using CoreTemplateStudio.Api;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Microsoft.Templates.Api.Test.Utilities
{
    public class TestClientProvider : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {
            
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
        }

        public void Dispose()
        {
            _server?.Dispose();
            Client?.Dispose();
        }
    }
}
