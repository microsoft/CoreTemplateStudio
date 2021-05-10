// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface ITemplatesService
    {
        IEnumerable<MetadataInfo> GetProjectTypes();

        IEnumerable<MetadataInfo> GetFrameworks(string projectType);

        IEnumerable<LayoutInfo> GetLayouts(string projectType, string frontEndFramework, string backEndFramework);

        IEnumerable<TemplateInfo> GetPages(string projectType, string frontEndFramework, string backEndFramework);

        IEnumerable<TemplateInfo> GetFeatures(string projectType, string frontEndFramework, string backEndFramework);

        IEnumerable<TemplateInfo> GetServices(string projectType, string frontEndFramework, string backEndFramework);

        IEnumerable<TemplateInfo> GetTestings(string projectType, string frontEndFramework, string backEndFramework);
    }
}
