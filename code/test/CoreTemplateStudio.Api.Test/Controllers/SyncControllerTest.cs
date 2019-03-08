using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Templates.Api.Test.ResponseModels;
using Microsoft.Templates.Api.Test.Utilities;

using Newtonsoft.Json;

using Xunit;

namespace Microsoft.Templates.Api.Test.Controllers
{
    [Trait("ExecutionSet", "Minimum")]
    public class SyncControllerTest
    {
         
        [Fact]
        public async void TestSync_ShouldHandleInvalidInput()
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
                    ApiResponse response = await GetResponseFromUrlPost(client, $"/api/sync?platform={plat}&path={ValidPath}");

                    response.StatusCode.Should().Be(400, "The platforms are invalid so shouldn't be successful response.");
                    response.Value["message"].Should().Be("invalid platform", "The platform is invalid so the message should reflect that");
                }

                // handle invalid paths
                foreach (string path in invalidPaths)
                {
                    ApiResponse response = await GetResponseFromUrlPost(client, $"/api/sync?platform=web&path={path}");

                    response.StatusCode.Should().Be(400, "The paths are invalid so shouldn't be successful response.");
                    response.Value["message"].Should().Be("invalid path", "The path is invalid so the message should reflect that");
                }

                // handle invalid platform + language combination
                ApiResponse resp = await GetResponseFromUrlPost(client, $"/api/sync/?platform=uwp&path={ValidPath}&language={InvalidUwp}");
                resp.StatusCode.Should().Be(400, "When passing the platform uwp, this language cannot be used.");
                resp.Value["message"].Should().Be("invalid language for this platform.", "The language is invalid so the message should reflect that");

                resp = await GetResponseFromUrlPost(client, $"/api/sync/?platform=web&path={ValidPath}&language={InvalidWeb}");
                resp.StatusCode.Should().Be(400, "When passing the platform web, this language cannot be used.");
                resp.Value["message"].Should().Be("invalid language for this platform.", "The language is invalid so the message should reflect that");
            }
        }

        [Fact]
        public async void TestSync_ShouldBeSuccessResponse()
        {
            const string ValidPath = ".";

            using (HttpClient client = new TestClientProvider().Client)
            {
                ApiResponse response = await GetResponseFromUrlPost(client, $"/api/sync?platform=web&path={ValidPath}");
                ApiResponse response2 = await GetResponseFromUrlPost(client, $"/api/sync?platform=uwp&path={ValidPath}&language=VisualBasic");
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
