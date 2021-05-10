using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WtsTelemetry.Services;

namespace WtsTelemetry
{
    public static class WebTSTelemetry
    {
        // Every minute: 0 * * * * *
        // Every 5 minutes: 0 */5 * * * *
        // Every day: 0 0 0 * * *
        // every monday at 09:00:00: 0 0 9 * * MON
        // every 1st of month (monthly): 0 0 0 1 * *
        [FunctionName("WebTSTelemetry")]
        public static async Task Run([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer, ILogger log)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

            var year = DateTime.Today.AddMonths(-1).Year;
            var month = DateTime.Today.AddMonths(-1).Month;
            var stringDate = $"{year}.{month.ToString("D2")}";

            var configService = new ConfigurationService("WebTS");
            var dataService = new ApplicationInsightService(configService);
            var githubService = new GithubService(configService, log);

            log.LogInformation($"WebTS: Get Application Insight data from {stringDate}");
            var WebTSData = await dataService.GetWebTSData(year, month);

            log.LogInformation($"WebTS: Create Md File");
            var mdText = WebTSData.ToMarkdown();

            log.LogInformation($"WebTS: Create Pull Request");
            await githubService.CreateTelemetryPullRequest(mdText, year, month);

            log.LogInformation($"WebTS: Finish");
        }
    }
}
