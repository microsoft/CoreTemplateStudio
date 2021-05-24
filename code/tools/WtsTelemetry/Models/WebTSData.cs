using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WebTSData
    {
        public WebTSFullStackWebData FullStackWeb { get; set; }
        public WebTSReactNativeData ReactNative { get; set; }
        public string Platform { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string ToMarkdown()
        {
            return new MarkdownBuilder()
                        .AddHeader("Web Template Studio", Year, Month)
                        .AddTable("Category", "Type", Platform)
                        .AddSectionTitle("Project Generation by category")
                        .AddCollapsible("Web Full Stack Generation", FullStackWeb.ToMarkdown())
                        .AddCollapsible("React Native Generation", ReactNative.ToMarkdown())
                        .GetText();
        }
    }
}
