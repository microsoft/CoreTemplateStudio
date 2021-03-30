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
            return new MarkdownBuilder("Windows Template Studio")
                        .AddHeader(Year, Month)
                        .AddTable("Category", "Type", Platform)
                        .AddTable("Project Type (Uwp)", "Project", Uwp.Project)
                        .AddTable("Framework (Uwp)", "Framework Type", Uwp.Frameworks)
                        .AddTable("Pages (Uwp)", "Pages", Uwp.Pages)
                        .AddTable("Features (Uwp)", "Features", Uwp.Features)
                        .AddTable("Services (Uwp)", "Services", Uwp.Services)
                        .AddTable("Testing (Uwp)", "Testing", Uwp.Testing)
                        .AddTable("Project Type (Wpf)", "Project", Wpf.Project)
                        .AddTable("Framework (Wpf)", "Framework Type", Wpf.Frameworks)
                        .AddTable("Pages (Wpf)", "Pages", Wpf.Pages)
                        .AddTable("Features (Wpf)", "Features", Wpf.Features)
                        .AddTable("Services (Wpf)", "Services", Wpf.Services)
                        .AddTable("Testing (Wpf)", "Services", Wpf.Testing)
                        .AddTable("Project Type (WinUI)", "Project", WinUI.Project)
                        .AddTable("Framework (WinUI)", "Framework Type", WinUI.Frameworks)
                        .AddTable("Pages (WinUI)", "Pages", WinUI.Pages)
                        .AddTable("Features (WinUI)", "Features", WinUI.Features)
                        .AddTable("Services (WinUI)", "Services", WinUI.Services)
                        .AddTable("Testing (WinUI)", "Services", WinUI.Testing)
                        .AddTable("AppModels (WinUI)", "Services", WinUI.AppModels)
                        .AddTable("Windows Template Studio entry point (Common)", "Entry point", entryPoint)
                        .AddTable("Programming languages (Common)", "Languages", Language)
                        .GetText();
        }
    }
}
