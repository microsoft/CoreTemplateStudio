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
            string baseMarkdown = new MarkdownBuilder()
                        .AddHeader("Windows Template Studio", Year, Month)
                        .AddTable("Category", "Type", Platform)
                        .AddTable("Windows Template Studio entry point (Common)", "Entry point", entryPoint)
                        .AddTable("Programming languages (Common)", "Languages", Language)
                        .GetText();

            return string.Concat(
                baseMarkdown,
                Uwp.ToMarkdown("Uwp"),
                Wpf.ToMarkdown("Wpf"),
                WinUI.ToMarkdown("WinUI"));
        }
    }
}
