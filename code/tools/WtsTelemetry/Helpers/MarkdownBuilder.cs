using System;
using System.Text;
using System.Linq;

namespace WtsTelemetry.Helpers
{
    public class MarkdownBuilder
    {
        private StringBuilder stringBuilder = new StringBuilder();
        private readonly string title;

        public MarkdownBuilder(string title)
        {
            this.title = title;
        }

        public MarkdownBuilder AddHeader(int year, int month)
        {
            stringBuilder.AppendLine($"# Telemetry for {title} - {year}.{month.ToString("D2")}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("As new features and pages roll out, percentages  will adjust.");
            stringBuilder.AppendLine();

            return this;
        }

        public MarkdownBuilder AddTable(string title, string firstColumnName, string json)
        {
            try
            {
                var data = json.ToQueryData();
                if (data.Any())
                {
                    stringBuilder.AppendLine($"## {title}");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"|{firstColumnName}|Percentage|");
                    stringBuilder.AppendLine("|:---|:---:|");

                    foreach (var value in data)
                    {
                        stringBuilder.AppendLine($"|{value.DisplayName}|{Math.Round(value.Value, 1)}%|");
                    }
                    stringBuilder.AppendLine();
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error to process {title} table.");
                stringBuilder.AppendLine($"Entry data: {json}.");
                stringBuilder.AppendLine($"Exception: {ex.Message}.");
                stringBuilder.AppendLine();
            }

            return this;
        }

        public string GetText() => stringBuilder.ToString();
    }
}
