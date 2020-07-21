using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WinTSData
    {
        public WinTSPlatformData Uwp { get; set; }
        public WinTSPlatformData Wpf { get; set; }
        public string entryPoint { get; set; }
        public string Language { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string ToMarkdown()
        {
            return new MarkdownBuilder("Windows Template Studio")
                        .AddHeader(Year, Month)
                        .AddTable("Project Type (Uwp)", "Project", Uwp.Project)
                        .AddTable("Project Type (Wpf)", "Project", Wpf.Project)
                        .AddTable("Framework (Uwp)", "Framework Type", Uwp.Frameworks)
                        .AddTable("Framework (Wpf)", "Framework Type", Wpf.Frameworks)
                        .AddTable("Pages (Uwp)", "Pages", Uwp.Pages)
                        .AddTable("Pages (Wpf)", "Pages", Wpf.Pages)
                        .AddTable("Features (Uwp)", "Features", Uwp.Features)
                        .AddTable("Features (Wpf)", "Features", Wpf.Features)
                        .AddTable("Services (Uwp)", "Services", Uwp.Services)
                        .AddTable("Services (Wpf)", "Services", Wpf.Services)
                        .AddTable("Testing (Uwp)", "Testing", Uwp.Testing)
                        .AddTable("Testing (Wpf)", "Services", Wpf.Testing)
                        .AddTable("Windows Template Studio entry point (Common)", "Entry point", entryPoint)
                        .AddTable("Programming languages (Common)", "Languages", Language)
                        .GetText();
        }
    }
}
