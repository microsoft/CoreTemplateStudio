using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WebTSReactNativeData
    {
        public string ProjectTypes { get; set; }
        public string Pages { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string ToMarkdown()
        {
            return new MarkdownBuilder()
                        .AddTable("Project Types", "Framework Type", ProjectTypes)
                        .AddTable("Pages", "Pages", Pages)
                        .GetText();
        }
    }
}
