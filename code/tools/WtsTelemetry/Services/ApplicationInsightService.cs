using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WtsTelemetry.Helpers;
using WtsTelemetry.Models;

namespace WtsTelemetry.Services
{
    public class ApplicationInsightService
    {
        private const string Uwp = "Uwp";
        private const string Wpf = "Wpf";
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
            var queries = new Queries(year, month);
            return new WinTSData
            {
                Uwp = GetWinTSPlatformData(Uwp, queries),
                Wpf = GetWinTSPlatformData(Wpf, queries),
                entryPoint = GetData(queries.EntryPoints),
                Language = GetData(queries.Languages),
                Year = year,
                Month = month
            };
        }

        private WinTSPlatformData GetWinTSPlatformData(string platform, Queries queries)
        {
            return new WinTSPlatformData
            {
                Project = GetData(queries.Projects(platform)),
                Frameworks = GetData(queries.Frameworks(platform)),
                Pages = GetData(queries.Pages(platform)),
                Features = GetData(queries.Features(platform)),
                Services = GetData(queries.Services(platform)),
                Testing = GetData(queries.Testing(platform)),
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
