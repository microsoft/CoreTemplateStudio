using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WtsTelemetry.Helpers;
using WtsTelemetry.Models;

namespace WtsTelemetry.Services
{
    public class ApplicationInsightService
    {
        private const string URL = "https://api.applicationinsights.io/v1/apps/{0}/query?query={1}";
        private readonly string appId;
        private readonly string apiKey;

        public ApplicationInsightService(ConfigurationService configService)
        {
            var config = configService.GetAppInsightConfig();
            appId = config.AppId;
            apiKey = config.AppKey;
        }

        public async Task<WinTSData> GetWinTSData(int year, int month)
        {
            var uwpQueries = new Queries(Platforms.Uwp, year, month);
            var wpfQueries = new Queries(Platforms.Wpf, year, month);
            var winUIQueries = new Queries(Platforms.WinUI, year, month, "Desktop");
            return new WinTSData
            {
                Uwp = await GetWinTSPlatformData(uwpQueries),
                Wpf = await GetWinTSPlatformData(wpfQueries),
                WinUI = await GetWinUIPlatformData(winUIQueries),
                entryPoint = await GetData(uwpQueries.EntryPoints),
                Language = await GetData(uwpQueries.Languages),
                Platform = await GetData(uwpQueries.Platforms),
                Year = year,
                Month = month
            };
        }

        public async Task<WebTSData> GetWebTSData(int year, int month)
        {
            var queries = new Queries(Platforms.Web, year, month);
            return new WebTSData
            {
                FrontendFrameworks = await GetData(queries.FrontendFrameworks),
                BackendFrameworks = await GetData(queries.BackendFrameworks),
                Pages = await GetData(queries.Pages),
                Services = await GetData(queries.Features),
                Year = year,
                Month = month
            };
        }

        private async Task<WinTSPlatformData> GetWinTSPlatformData(Queries queries)
        {
            return new WinTSPlatformData
            {
                Project = await GetData(queries.Projects),
                Frameworks = await GetData(queries.Frameworks),
                Pages = await GetData(queries.Pages),
                Features = await GetData(queries.Features),
                Services = await GetData(queries.Services),
                Testing = await GetData(queries.Testing),
            };
        }

        private async Task<WinUIPlatformData> GetWinUIPlatformData(Queries queries)
        {
            var appModels = new string[] { "Desktop", "Uwp" };
            return new WinUIPlatformData
            {
                Project = await GetData(queries.Projects),
                Frameworks = await GetData(queries.Frameworks),
                Pages = await GetData(queries.Pages),
                Features = await GetData(queries.Features),
                Services = await GetData(queries.Services),
                Testing = await GetData(queries.Testing),
                AppModels = await GetData(queries.AppModels(appModels)),
            };
        }

        private async Task<string> GetData(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var req = string.Format(URL, appId, query);
            HttpResponseMessage response = await client.GetAsync(req);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new Exception($"Error getting Application Insights data: {response.ReasonPhrase}");
        }
    }
}
