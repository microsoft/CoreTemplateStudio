using System;
using System.Collections.Generic;
using System.Text;
using WtsTelemetry.Helpers;

namespace WtsTelemetry.Models
{
    public class WinUIPlatformData: WinTSPlatformData
    {
        public string AppModels { get; set; }

        public override string ToMarkdown(string title)
        {
            string winUIMarkdown = new MarkdownBuilder()
                .AddTable("AppModels", "Services", AppModels)
                .GetText();

            return string.Concat(base.ToMarkdown(title), winUIMarkdown);
        }
    }
}
