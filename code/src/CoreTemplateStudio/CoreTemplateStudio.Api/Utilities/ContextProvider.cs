// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Api.Utilities
{
    public class ContextProvider : IContextProvider
    {
        public string ProjectName { get; set; } = string.Empty;

        public string SafeProjectName => ProjectName.MakeSafeProjectName();

        public string GenerationOutputPath { get; set; }

        public string DestinationPath { get; set; }

        public ProjectInfo ProjectInfo { get; set; } = new ProjectInfo();

        public List<string> FilesToOpen { get; set; } = new List<string>();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; set; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; set; } = new Dictionary<string, List<MergeInfo>>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; set; } = new Dictionary<ProjectMetricsEnum, double>();
    }
}
