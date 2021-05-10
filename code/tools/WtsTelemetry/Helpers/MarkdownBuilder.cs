using System;
using System.Text;
using System.Linq;

namespace WtsTelemetry.Helpers
{
    public class MarkdownBuilder
    {
        private StringBuilder stringBuilder = new StringBuilder();

        public MarkdownBuilder AddHeader(string title, int year, int month)
        {
            stringBuilder.AppendLine($"# Telemetry for {title} - {year}.{month.ToString("D2")}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("As new features and pages roll out, percentages  will adjust.");
            stringBuilder.AppendLine();

            return this;
        }

        public MarkdownBuilder AddSectionTitle(string title)
        {
            stringBuilder.AppendLine($"### {title}");
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
                    AddSectionTitle(title);
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

        public MarkdownBuilder AddCollapsible(string summary, string data)
        {
            stringBuilder.AppendLine($"<details>");
            stringBuilder.AppendLine($"<summary>{summary}</summary>");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{data}");
            stringBuilder.AppendLine($"</details>");
            stringBuilder.AppendLine();
            return this;
        }

        public string GetText() => stringBuilder.ToString();
    }
}
