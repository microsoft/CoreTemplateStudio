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

        public SendGridConfig GetSendGridConfig()
        {
            return new SendGridConfig
            {
                Subject = Environment.GetEnvironmentVariable($"{filter}-SendGrid:Subject"),
                From = Environment.GetEnvironmentVariable($"{filter}-SendGrid:From"),
                To = Environment.GetEnvironmentVariable($"{filter}-SendGrid:To")
                        .Split(';')
                        .Select(email => email.Trim()),
            };
        }
    }
}
