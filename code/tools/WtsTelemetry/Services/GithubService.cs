using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WtsTelemetry.Services
{
    public class GithubService
    {
        private readonly ILogger log;
        private readonly string accessToken;
        private readonly string repoOwner;
        private readonly string repoName;
        private readonly string productHeaderValue = "wtsTelemetry";

        private readonly string getBranchUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/git/refs/heads/%%BRANCH%%";
        private readonly string createBranchUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/git/refs";
        private readonly string getFileUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/contents/%%FILEPATH%%?ref=%%BRANCH%%";
        private readonly string updateFileUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/contents/%%FILEPATH%%";
        private readonly string createPullRequestUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/pulls";

        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };

        public GithubService(ConfigurationService configService, ILogger logger)
        {
            var config = configService.GetGithubConfig();
            accessToken = config.AccessToken;
            repoOwner = config.Owner;
            repoName = config.RepositoryName;
            log = logger;
        }

        public async Task CreateTelemetryPullRequest(string telemetryData, int year, int month)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            var baseBranchName = "dev";
            var newBranchName = $"UpdateTelemetryData-{monthName}-{year}";
            var telemetryFilePath = "docs/telemetryData.md";
            var commitMessage = $"Update telemetry with data from {monthName}";
            var pullRequestTitle = $"Update telemetry data with data from {monthName}";

            try
            {
                if ((await GetBranch(newBranchName)) is null) {

                    var baseBranch = await GetBranch(baseBranchName);
                    await CreateNewBranch(baseBranch.Object.Sha, newBranchName);

                }

                var telemetryFile = await GetFile(telemetryFilePath, newBranchName);
                await UpdateFile(telemetryFilePath, telemetryFile.Sha, newBranchName, telemetryData, commitMessage);
                await CreatePullRequest(newBranchName, baseBranchName, pullRequestTitle);
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<BranchData> GetBranch(string branchName)
        {
            try
            {
                string url = getBranchUrl
                    .Replace("%%REPO%%", repoName)
                    .Replace("%%OWNER%%", repoOwner)
                    .Replace("%%BRANCH%%", branchName);

                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(productHeaderValue)));

                var branch = await client.GetFromJsonAsync<BranchData>(url, jsonSerializerOptions);
                return branch;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        private async Task<bool> CreateNewBranch(string baseBranchSha, string newBranchName)
        {
            string url = createBranchUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(productHeaderValue)));

            var newBranchData = new { 
                Ref = $"refs/heads/{newBranchName}",
                Sha = baseBranchSha
            };

            var resp = await client.PostAsJsonAsync(url, newBranchData, jsonSerializerOptions);
            return resp.StatusCode == HttpStatusCode.Created;
        }

        private async Task<FileData> GetFile(string filePath, string branch)
        {
            string url = getFileUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner)
                .Replace("%%FILEPATH%%", filePath)
                .Replace("%%BRANCH%%", branch);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(productHeaderValue)));

            var fileData = await client.GetFromJsonAsync<FileData>(url, jsonSerializerOptions);
            return fileData;
        }

        private async Task<bool> UpdateFile(string filePath, string fileSha, string branchName, string fileContent, string commitMessage)
        {
            string url = updateFileUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner)
                .Replace("%%FILEPATH%%", filePath)
                .Replace("%%BRANCH%%", branchName);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(productHeaderValue)));

            var newFileData = new
            {
                Message = commitMessage,
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent)),
                Sha = fileSha,
                Branch = branchName
            };

            var resp = await client.PutAsJsonAsync(url, newFileData, jsonSerializerOptions);
            return resp.StatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> CreatePullRequest(string headBranch, string baseBranch, string titlePR)
        {
            string url = createPullRequestUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(productHeaderValue)));

            var newPRData = new
            {
                Title = titlePR,
                Head = headBranch,
                Base = baseBranch,
            };

            var resp = await client.PostAsJsonAsync(url, newPRData, jsonSerializerOptions);
            return resp.StatusCode == HttpStatusCode.Created;
        }
        
        private class BranchData
        {
            public BranchObjectData Object { get; set; }
        }

        private class BranchObjectData
        {
            public string Sha { get; set; }
        }

        private class FileData
        {
            public string Sha { get; set; }
        }
    }
}
