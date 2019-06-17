using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Services
{
    public class TemplatesService : ITemplatesService
    {   
        public IEnumerable<TemplateInfo> GetFeatures(string projectType, string frontEndFramework, string backEndFramework)
        {
            if (frontEndFramework == null && backEndFramework == null)
            {
                throw new Exception(StringRes.BadReqNoBackendOrFrontend);
            }

            var platform = GenContext.CurrentPlatform;
            var features = GenContext.ToolBox.Repo.GetTemplatesInfo(
                                                                TemplateType.Feature,
                                                                platform,
                                                                projectType,
                                                                frontEndFramework,
                                                                backEndFramework);

            return features;
        }

        public IEnumerable<MetadataInfo> GetFrameworks(string projectType)
        {
            if (string.IsNullOrEmpty(projectType))
            {
                throw new Exception(StringRes.BadReqInvalidProjectType);
            }

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
            if (frontEndFramework == null && backEndFramework == null)
            {
                throw new Exception(StringRes.BadReqNoBackendOrFrontend);
            }

            var platform = GenContext.CurrentPlatform;
            var pages = GenContext.ToolBox.Repo.GetTemplatesInfo(
                                                                TemplateType.Page,
                                                                platform,
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
    }
}
