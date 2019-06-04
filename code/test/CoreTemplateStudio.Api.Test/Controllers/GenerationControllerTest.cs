// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CoreTemplateStudio.Api;
using CoreTemplateStudio.Api.Models.Generation;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Templates.Api.Test.ResponseModels;
using Microsoft.Templates.Api.Test.Utilities;
using Microsoft.Templates.Core.Locations;
using Newtonsoft.Json;

using Xunit;

namespace Microsoft.Templates.Api.Test.Controllers
{
    [Trait("ExecutionSet", "Minimum")]
    public class GenerationControllerTest : IDisposable
    {
        private TestServer testServer;

        [Fact]
        public async void TestGenerate_ShouldBeSuccessResponse()
        {
            Directory.CreateDirectory("Test");
            var message = string.Empty;

            var connection = CreateConnection();

            connection.On<string>("genMessage", (msg) =>
            {
                message = msg;
            });

            await connection.StartAsync();
            await connection.InvokeAsync("SyncTemplates", ".");
            await connection.InvokeAsync("Generate", testGeneration);
            await connection.StopAsync();

            message.Should().Be("Creating project 'Test'...", "Generation has finished with success");
        }

        private HubConnection CreateConnection()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            return new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost:5000/corehub",
                    o => o.HttpMessageHandlerFactory = _ => testServer.CreateHandler())
                .Build();
        }

        private GenerationData testGeneration = new GenerationData()
        {
            ProjectName = "Test",
            GenPath = ".",
            ProjectType = "TestProjectType",
            FrontendFramework = "TestFramework",
            BackendFramework = string.Empty,
            HomeName = "HomeName",
            Language = "Any",
            Platform = "Web",
            Pages = new List<GenerationItem>(),
            Features = new List<GenerationItem>(),
        };

        private async Task<ApiResponse> GetResponseFromUrlPost(HttpClient client, string url)
        {
            HttpResponseMessage httpResponse = await client.PostAsync(url, null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(content);
        }

        public void Dispose()
        {
            testServer?.Dispose();
        }
    }
}
