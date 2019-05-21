// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Templates.Api.Test.ResponseModels;
using Microsoft.Templates.Api.Test.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Test.TestFakes;

using Newtonsoft.Json;

using Xunit;

namespace Microsoft.Templates.Api.Test.Controllers
{
    [Trait("ExecutionSet", "Minimum")]
    public class ProjectTypeControllerTest
    {
        [Fact]
        public async void TestProjectType_SyncNotCalled()
        {
            using (HttpClient client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync($"/api/projectType");
                response.StatusCode.Should().Be(403, "The sync endpoint has been called so the projectType cannot be called");
            }
        }

        private async Task<ApiResponse> GetResponseFromUrl(HttpClient client, string url)
        {
            HttpResponseMessage httpResponse = await client.GetAsync(url);
            string content = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(content);
        }
    }
}
