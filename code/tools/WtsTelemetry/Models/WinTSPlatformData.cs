using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WinTSPlatformData
    {
        public string Project { get; set; }
        public string Frameworks { get; set; }
        public string Pages { get; set; }
        public string Features { get; set; }
        public string Services { get; set; }
        public string Testing { get; set; }

        public virtual string ToMarkdown()
        {
            return new MarkdownBuilder()
                .AddTable("Project Type", "Project", Project)
                .AddTable("Framework", "Framework Type", Frameworks)
                .AddTable("Pages", "Pages", Pages)
                .AddTable("Features", "Features", Features)
                .AddTable("Services", "Services", Services)
                .AddTable("Testing", "Testing", Testing)
                .GetText();
        }
    }
}
