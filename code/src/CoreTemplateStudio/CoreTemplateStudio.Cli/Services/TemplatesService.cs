// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Services
{
    public class TemplatesService : ITemplatesService
    {
        public IEnumerable<TemplateInfo> GetFeatures(string projectType, string frontEndFramework, string backEndFramework)
        {
            var features = GetTemplateItems(
                                            TemplateType.Feature,
                                            projectType,
                                            frontEndFramework,
                                            backEndFramework);

            return features;
        }

        public IEnumerable<MetadataInfo> GetFrameworks(string projectType)
        {
            var platform = GenContext.CurrentPlatform;
            var frontendFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType);
            var backendFrameworks = GenContext.ToolBox.Repo.GetBackEndFrameworks(platform, projectType);

            var targetFrameworks = new List<MetadataInfo>();
            targetFrameworks.AddRange(frontendFrameworks);
            targetFrameworks.AddRange(backendFrameworks);

            return targetFrameworks;
        }

        public IEnumerable<TemplateInfo> GetPages(string projectType, string frontEndFramework, string backEndFramework)
        {
            var pages = GetTemplateItems(
                                        TemplateType.Page,
                                        projectType,
                                        frontEndFramework,
                                        backEndFramework);

            return pages;
        }

        public IEnumerable<MetadataInfo> GetProjectTypes()
        {
            var platform = GenContext.CurrentPlatform;
            var result = GenContext.ToolBox.Repo.GetProjectTypes(platform);

            return result;
        }

        public IEnumerable<LayoutInfo> GetLayouts(string projectType, string frontEndFramework, string backEndFramework)
        {
            var platform = GenContext.CurrentPlatform;
            var layouts = GenContext.ToolBox.Repo.GetLayoutTemplates(platform, projectType, frontEndFramework, backEndFramework);

            return layouts;
        }

        public IEnumerable<TemplateInfo> GetServices(string projectType, string frontEndFramework, string backEndFramework)
        {
            var pages = GetTemplateItems(
                                        TemplateType.Service,
                                        projectType,
                                        frontEndFramework,
                                        backEndFramework);

            return pages;
        }

        public IEnumerable<TemplateInfo> GetTestings(string projectType, string frontEndFramework, string backEndFramework)
        {
            var pages = GetTemplateItems(
                                        TemplateType.Testing,
                                        projectType,
                                        frontEndFramework,
                                        backEndFramework);

            return pages;
        }

        private IEnumerable<TemplateInfo> GetTemplateItems(TemplateType templateType, string projectType, string frontEndFramework, string backEndFramework)
        {
            var platform = GenContext.CurrentPlatform;
            var templateItems = GenContext.ToolBox.Repo.GetTemplatesInfo(
                                                                templateType,
                                                                platform,
                                                                projectType,
                                                                frontEndFramework,
                                                                backEndFramework);

            return templateItems;
        }
    }
}
