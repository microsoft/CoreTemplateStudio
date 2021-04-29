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
        private readonly string accessToken;
        private readonly string repoOwner;
        private readonly string repoName;

        private readonly string getBranchUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/git/refs/heads/%%BRANCH%%";
        private readonly string createBranchUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/git/refs";
        private readonly string getFileUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/contents/%%FILEPATH%%?ref=%%BRANCH%%";
        private readonly string updateFileUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/contents/%%FILEPATH%%";
        private readonly string createPullRequestUrl = "https://api.github.com/repos/%%OWNER%%/%%REPO%%/pulls";

        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };

        public GithubService(ConfigurationService configService)
        {
            var config = configService.GetGithubConfig();
            accessToken = config.AccessToken;
            repoOwner = config.Owner;
            repoName = config.RepositoryName;
        }

        public async Task CreateTelemetryPullRequest(string telemetryData, int year, int month)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            var baseBranchName = "dev";
            var newBranchName = $"UpdateTelemetryData-{month}-{year}";
            var telemetryFilePath = "docs/telemetryData.md";
            var commitMessage = $"Update telemetry with Data from {monthName}";
            var pullRequestTitle = $"Update telemetry data with data from {monthName}";

            var baseBrachSha = await GetBranchSha(baseBranchName);
            var isNewBranchCreated = await CreateNewBranch(baseBrachSha, newBranchName);
            var telemetryFileSha = await GetFileSha(telemetryFilePath, newBranchName);
            var isTelemetryFileUpdated = await UpdateFile(telemetryFilePath, telemetryFileSha, newBranchName, telemetryData, commitMessage);
            var isPullRequestCreated = await CreatePullRequest(newBranchName, baseBranchName, pullRequestTitle);
        }

        private async Task<string> GetBranchSha(string branchName)
        {
            string url = getBranchUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner)
                .Replace("%%BRANCH%%", branchName);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("MyAmazingApp")));

            var resp = await client.GetFromJsonAsync<BranchData>(url, jsonSerializerOptions);
            return resp.Object.Sha;
        }

        private async Task<bool> CreateNewBranch(string baseBranchSha, string newBranchName)
        {
            string url = createBranchUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("MyAmazingApp")));

            var newBranchData = new { 
                Ref = $"refs/heads/{newBranchName}",
                Sha = baseBranchSha
            };

            var resp = await client.PostAsJsonAsync(url, newBranchData, jsonSerializerOptions);
            return resp.StatusCode == HttpStatusCode.Created;
        }

        private async Task<string> GetFileSha(string filePath, string branch)
        {
            string url = getFileUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner)
                .Replace("%%FILEPATH%%", filePath)
                .Replace("%%BRANCH%%", branch);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("MyAmazingApp")));

            var fileData = await client.GetFromJsonAsync<FileData>(url, jsonSerializerOptions);
            return fileData.Sha;
            
            /*
            var resp = await client.GetAsync(url);
            if(resp.StatusCode == HttpStatusCode.OK)
            {
                string data = await resp.Content.ReadAsStringAsync();
                var branchObject = JsonSerializer.Deserialize<FileData>(data, jsonSerializerOptions);
                return branchObject.Sha;
            }

            return string.Empty;*/
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
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("MyAmazingApp")));

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

        private async Task<bool> CreatePullRequest(string headBranch, string baseBranch, string titlePR, string description = null)
        {
            string url = createPullRequestUrl
                .Replace("%%REPO%%", repoName)
                .Replace("%%OWNER%%", repoOwner);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("MyAmazingApp")));

            var newPRData = new
            {
                Title = titlePR,
                HeadBranch = headBranch,
                BaseBranch = baseBranch,
                Description = description ?? string.Empty
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
