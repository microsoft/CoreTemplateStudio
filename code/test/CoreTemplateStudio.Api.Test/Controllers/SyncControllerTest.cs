using Microsoft.Templates.Api.Test.Utilities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Newtonsoft.Json;
using Microsoft.Templates.Api.Test.ResponseModels;
using FluentAssertions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Api.Test.Controllers
{
    [Trait("ExecutionSet", "Minimum")]
    public class SyncControllerTest
    {
         
        [Fact]
        public void TestSync_ShouldHandleInvalidInput()
        {
            List<string> invalidPlatforms = new List<string>() { "uw", "we", "windows", null };
            List<string> invalidPaths = (new List<string>() { "../../", null, "test", "../../../test/CoreTemplateStudio.Api.Test/" });
            const string ValidPath = ".";
            const string InvalidWeb = "VisualBasic";
            const string InvalidUwp = "Any";


            using (HttpClient client = new TestClientProvider().Client)
            {
                // handle invalid platforms
                foreach (string plat in invalidPlatforms)
                {
                    ApiResponse response = GetResponseFromUrlPost(client, $"/api/sync?platform={plat}&path={ValidPath}").Result;

                    response.StatusCode.Should().Be(400, "The platforms are invalid so shouldn't be successful response.");
                    response.Value["message"].Should().Be("invalid platform", "The platform is invalid so the message should reflect that");
                }

                // handle invalid paths
                foreach (string path in invalidPaths)
                {
                    ApiResponse response = GetResponseFromUrlPost(client, $"/api/sync?platform=web&path={path}").Result;

                    response.StatusCode.Should().Be(400, "The paths are invalid so shouldn't be successful response.");
                    response.Value["message"].Should().Be("invalid path", "The path is invalid so the message should reflect that");
                }

                // handle invalid platform + language combination
                ApiResponse resp = GetResponseFromUrlPost(client, $"/api/sync/?platform=uwp&path={ValidPath}&language={InvalidUwp}").Result;
                resp.StatusCode.Should().Be(400, "When passing the platform uwp, this language cannot be used.");
                resp.Value["message"].Should().Be("invalid language for this platform.", "The language is invalid so the message should reflect that");

                resp = GetResponseFromUrlPost(client, $"/api/sync/?platform=web&path={ValidPath}&language={InvalidWeb}").Result;
                resp.StatusCode.Should().Be(400, "When passing the platform web, this language cannot be used.");
                resp.Value["message"].Should().Be("invalid language for this platform.", "The language is invalid so the message should reflect that");
            }
        }

        [Fact]
        public void TestSync_ShouldBeSuccessResponse()
        {
            const string ValidPath = ".";

            using (HttpClient client = new TestClientProvider().Client)
            {
                ApiResponse response = GetResponseFromUrlPost(client, $"/api/sync?platform=web&path={ValidPath}").Result;
                ApiResponse response2 = GetResponseFromUrlPost(client, $"/api/sync?platform=uwp&path={ValidPath}&language=VisualBasic").Result;
                response.StatusCode.Should().Be(200, "The parameters passed are valid, so the response should be successful");
                response2.StatusCode.Should().Be(200, "The parameters passed are valid, so the response should be successful");
            }
        }

        private async Task<ApiResponse> GetResponseFromUrlPost(HttpClient client, string url)
        {
            HttpResponseMessage httpResponse = await client.PostAsync(url, null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(content);
        }
    }
}
