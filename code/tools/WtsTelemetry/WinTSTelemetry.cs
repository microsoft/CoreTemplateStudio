using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WtsTelemetry
{
    public static class WinTSTelemetry
    {
        [FunctionName("WinTSTelemetry")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"WinTSTelemetry trigger function executed at: {DateTime.Now}");
        }
    }
}
