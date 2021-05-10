using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WtsTelemetry.Services;
using System.Threading.Tasks;
using System.Globalization;

namespace WtsTelemetry
{
    public static class WinTSTelemetry
    {
        // Every minute: 0 * * * * *
        // Every 5 minutes: 0 */5 * * * *
        // Every day: 0 0 0 * * *
        // every monday at 09:00:00: 0 0 9 * * MON
        // every 1st of month (monthly): 0 0 0 1 * *
        [FunctionName("WinTSTelemetry")]
        public static async Task Run([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer, ILogger log)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

            var year = DateTime.Today.AddMonths(-1).Year;
            var month = DateTime.Today.AddMonths(-1).Month;
            var stringDate = $"{year}.{month.ToString("D2")}";

            var configService = new ConfigurationService("WinTS");
            var dataService = new ApplicationInsightService(configService);
            var githubService = new GithubService(configService, log);

            log.LogInformation($"WinTS: Get Application Insight data from {stringDate}");
            var winTSData = await dataService.GetWinTSData(year, month);

            log.LogInformation($"WinTS: Create Md File");
            var mdText = winTSData.ToMarkdown();

            log.LogInformation($"WinTS: Create Pull Request");
            await githubService.CreateTelemetryPullRequest(mdText, year, month);

            log.LogInformation($"WinTS: Finish");
        }
    }
}
