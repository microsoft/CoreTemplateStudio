using System.Collections.Generic;
using CoreTemplateStudio.Core.Test.Extensions;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.Test.TestFakes
{
    class TestContextProvider : IContextProvider
    {
        public string ProjectName { get; set; }

        public string SafeProjectName => ProjectName.MakeSafeProjectName();

        public string GenerationOutputPath { get; set; }

        public string DestinationPath { get; set; }

        public ProjectInfo ProjectInfo { get; set; } = new ProjectInfo();

        public List<string> FilesToOpen { get; } = new List<string>();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();
    }
    
}
