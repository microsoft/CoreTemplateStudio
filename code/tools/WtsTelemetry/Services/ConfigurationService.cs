using System;
using System.Linq;
using WtsTelemetry.Models;

namespace WtsTelemetry.Services
{
    public class ConfigurationService
    {
        private readonly string filter;

        public ConfigurationService(string filter)
        {
            this.filter = filter;
        }

        public AppInsightConfig GetAppInsightConfig()
        {
            return new AppInsightConfig
            {
                AppId = Environment.GetEnvironmentVariable($"{filter}-AppId"),
                AppKey = Environment.GetEnvironmentVariable($"{filter}-ApiKey"),
            };
        }

        public GithubConfig GetGithubConfig()
        {
            return new GithubConfig
            {
                AccessToken = Environment.GetEnvironmentVariable("Github-AccessToken"),
                Owner = Environment.GetEnvironmentVariable("Github-RepositoryOwner"),
                RepositoryName = Environment.GetEnvironmentVariable($"{filter}-RepositoryName"),
            };
        }
    }
}
