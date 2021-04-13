using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public WinTSData GetWinTSData(int year, int month)
        {
            var uwpQueries = new Queries(Platforms.Uwp, year, month);
            var wpfQueries = new Queries(Platforms.Wpf, year, month);
            var winUIQueries = new Queries(Platforms.WinUI, year, month, "Desktop");
            return new WinTSData
            {
                Uwp = GetWinTSPlatformData(uwpQueries),
                Wpf = GetWinTSPlatformData(wpfQueries),
                WinUI = GetWinUIPlatformData(winUIQueries),
                entryPoint = GetData(uwpQueries.EntryPoints),
                Language = GetData(uwpQueries.Languages),
                Platform = GetData(uwpQueries.Platforms),
                Year = year,
                Month = month
            };
        }

        public WebTSData GetWebTSData(int year, int month)
        {
            var queries = new Queries(Platforms.Web, year, month);
            return new WebTSData
            {
                FrontendFrameworks = GetData(queries.FrontendFrameworks),
                BackendFrameworks = GetData(queries.BackendFrameworks),
                Pages = GetData(queries.Pages),
                Services = GetData(queries.Features),
                Year = year,
                Month = month
            };
        }

        private WinTSPlatformData GetWinTSPlatformData(Queries queries)
        {
            return new WinTSPlatformData
            {
                Project = GetData(queries.Projects),
                Frameworks = GetData(queries.Frameworks),
                Pages = GetData(queries.Pages),
                Features = GetData(queries.Features),
                Services = GetData(queries.Services),
                Testing = GetData(queries.Testing),
            };
        }

        private WinUIPlatformData GetWinUIPlatformData(Queries queries)
        {
            var appModels = new string[] { "Desktop", "Uwp" };
            return new WinUIPlatformData
            {
                Project = GetData(queries.Projects),
                Frameworks = GetData(queries.Frameworks),
                Pages = GetData(queries.Pages),
                Features = GetData(queries.Features),
                Services = GetData(queries.Services),
                Testing = GetData(queries.Testing),
                AppModels = GetData(queries.AppModels(appModels)),
            };
        }

        private string GetData(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var req = string.Format(URL, appId, query);
            HttpResponseMessage response = client.GetAsync(req).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return response.ReasonPhrase;
        }
    }
}
