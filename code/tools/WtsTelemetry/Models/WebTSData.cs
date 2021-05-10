using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WebTSData
    {
        public string FrontendFrameworks { get; set; }
        public string BackendFrameworks { get; set; }
        public string Pages { get; set; }
        public string Services { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string ToMarkdown()
        {
            return new MarkdownBuilder()
                        .AddHeader("Web Template Studio", Year, Month)
                        .AddTable("Frontend Frameworks", "Framework Type", FrontendFrameworks)
                        .AddTable("Backend Frameworks", "Framework Type", BackendFrameworks)
                        .AddTable("Pages", "Pages", Pages)
                        .AddTable("Services", "Services", Services)
                        .GetText();
        }
    }
}
