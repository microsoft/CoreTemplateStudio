using System;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public string GetData(string query)
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
