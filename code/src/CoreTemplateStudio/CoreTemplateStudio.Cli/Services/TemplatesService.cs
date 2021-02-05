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
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
                FrontEndFramework = frontEndFramework,
                BackEndFramework = backEndFramework,
            };
            var features = GetTemplateItems(TemplateType.Feature, context);

            return features;
        }

        public IEnumerable<MetadataInfo> GetFrameworks(string projectType)
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
            };

            var frontendFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context);
            var backendFrameworks = GenContext.ToolBox.Repo.GetBackEndFrameworks(context);

            var targetFrameworks = new List<MetadataInfo>();
            targetFrameworks.AddRange(frontendFrameworks);
            targetFrameworks.AddRange(backendFrameworks);

            return targetFrameworks;
        }

        public IEnumerable<TemplateInfo> GetPages(string projectType, string frontEndFramework, string backEndFramework)
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
                FrontEndFramework = frontEndFramework,
                BackEndFramework = backEndFramework,
            };

            var pages = GetTemplateItems(TemplateType.Page, context);

            return pages;
        }

        public IEnumerable<MetadataInfo> GetProjectTypes()
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform);
            var result = GenContext.ToolBox.Repo.GetProjectTypes(context);

            return result;
        }

        public IEnumerable<LayoutInfo> GetLayouts(string projectType, string frontEndFramework, string backEndFramework)
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
                FrontEndFramework = frontEndFramework,
                BackEndFramework = backEndFramework,
            };
            var layouts = GenContext.ToolBox.Repo.GetLayoutTemplates(context);

            return layouts;
        }

        public IEnumerable<TemplateInfo> GetServices(string projectType, string frontEndFramework, string backEndFramework)
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
                FrontEndFramework = frontEndFramework,
                BackEndFramework = backEndFramework,
            };

            var pages = GetTemplateItems(TemplateType.Service, context);

            return pages;
        }

        public IEnumerable<TemplateInfo> GetTestings(string projectType, string frontEndFramework, string backEndFramework)
        {
            var context = new UserSelectionContext(GenContext.CurrentLanguage, GenContext.CurrentPlatform)
            {
                ProjectType = projectType,
                FrontEndFramework = frontEndFramework,
                BackEndFramework = backEndFramework,
            };

            var pages = GetTemplateItems(TemplateType.Testing, context);

            return pages;
        }

        private IEnumerable<TemplateInfo> GetTemplateItems(TemplateType templateType, UserSelectionContext context)
        {
            var platform = GenContext.CurrentPlatform;
            var templateItems = GenContext.ToolBox.Repo.GetTemplatesInfo(templateType, context);

            return templateItems;
        }
    }
}
