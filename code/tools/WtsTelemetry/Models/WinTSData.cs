using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WinTSData
    {
        public WinTSPlatformData Uwp { get; set; }
        public WinTSPlatformData Wpf { get; set; }
        public WinUIPlatformData WinUI { get; set; }
        public string entryPoint { get; set; }
        public string Language { get; set; }
        public string Platform { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string ToMarkdown()
        {
            return new MarkdownBuilder()
                        .AddHeader("Windows Template Studio", Year, Month)
                        .AddTable("Category", "Type", Platform)
                        .AddSectionTitle("Project Generation by category")
                        .AddCollapsible("Uwp Project Generation", Uwp.ToMarkdown())
                        .AddCollapsible("Wpf Project Generation", Wpf.ToMarkdown())
                        .AddCollapsible("WinUI Project Generation", WinUI.ToMarkdown())
                        .AddTable("Windows Template Studio entry point", "Entry point", entryPoint)
                        .AddTable("Programming languages", "Languages", Language)
                        .GetText();
        }
    }
}
