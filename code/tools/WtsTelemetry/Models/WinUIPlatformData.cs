using System;
using System.Collections.Generic;
using System.Text;
using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WinUIPlatformData: WinTSPlatformData
    {
        public string AppModels { get; set; }

        public override string ToMarkdown()
        {
            return new MarkdownBuilder()
                .AddTable("App Model", "App Model", AppModels)
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
