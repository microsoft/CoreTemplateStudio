using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using WtsTelemetry.Helpers;
using WtsTelemetry.Services;

namespace WtsTelemetry
{
    public static class WinTSTelemetry
    {
        private const string Uwp = "Uwp";
        private const string Wpf = "Wpf";

        // Every minute: 0 * * * * *
        // Every 5 minutes: 0 */5 * * * *
        // Every day: 0 0 0 * * *
        // every monday at 09:00:00: 0 0 9 * * MON
        // every 1st of month (monthly): 0 0 0 1 * *
        [FunctionName("WinTSTelemetry")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log, [SendGrid] out SendGridMessage message)
        {
            var configService = new ConfigurationService("WinTS");

            var year = DateTime.Today.AddMonths(-1).Year;
            var month = DateTime.Today.AddMonths(-1).Month;
            var stringDate = $"{year}.{month.ToString("D2")}";
            var queries = new QueryService(year, month);

            var dataService = new ApplicationInsightService(configService);
            var emailService = new EmailService(configService);

            log.LogInformation($"WinTS: Get Application Insight data from {stringDate}");
            var projectDataUwp = dataService.GetData(queries.Projects(Uwp));
            var projectDataWpf = dataService.GetData(queries.Projects(Wpf));
            var frameworksDataUwp = dataService.GetData(queries.Frameworks(Uwp));
            var frameworksDataWpf = dataService.GetData(queries.Frameworks(Wpf));
            var pagesDataUwp = dataService.GetData(queries.Pages(Uwp));
            var pagesDataWpf = dataService.GetData(queries.Pages(Wpf));
            var featuresDataUwp = dataService.GetData(queries.Features(Uwp));
            var featuresDataWpf = dataService.GetData(queries.Features(Wpf));
            var servicesDataUwp = dataService.GetData(queries.Services(Uwp));
            var servicesDataWpf = dataService.GetData(queries.Services(Wpf));
            var testingDataUwp = dataService.GetData(queries.Testing(Uwp));
            var testingDataWpf = dataService.GetData(queries.Testing(Wpf));
            var entryPointData = dataService.GetData(queries.EntryPoints);
            var languageData = dataService.GetData(queries.Languages);

            log.LogInformation($"WinTS: Create Md File");
            var mdText = new MarkdownBuilder("Windows Template Studio")
                        .AddHeader(year, month)
                        .AddTable("Project Type (Uwp)", "Project", projectDataUwp)
                        .AddTable("Project Type (Wpf)", "Project", projectDataWpf)
                        .AddTable("Framework (Uwp)", "Framework Type", frameworksDataUwp)
                        .AddTable("Framework (Wpf)", "Framework Type", frameworksDataWpf)
                        .AddTable("Pages (Uwp)", "Pages", pagesDataUwp)
                        .AddTable("Pages (Wpf)", "Pages", pagesDataWpf)
                        .AddTable("Features (Uwp)", "Features", featuresDataUwp)
                        .AddTable("Features (Wpf)", "Features", featuresDataWpf)
                        .AddTable("Services (Uwp)", "Services", servicesDataUwp)
                        .AddTable("Services (Wpf)", "Services", servicesDataWpf)
                        .AddTable("Testing (Uwp)", "Testing", testingDataUwp)
                        .AddTable("Testing (Wpf)", "Services", testingDataWpf)
                        .AddTable("Windows Template Studio entry point (Common)", "Entry point", entryPointData)
                        .AddTable("Programming languages (Common)", "Languages", languageData)
                        .GetText();


            log.LogInformation($"WinTS: send data mail");
            message = emailService.CreateMail(mdText, stringDate);

            log.LogInformation($"WinTS: Finish");
        }
    }
}
