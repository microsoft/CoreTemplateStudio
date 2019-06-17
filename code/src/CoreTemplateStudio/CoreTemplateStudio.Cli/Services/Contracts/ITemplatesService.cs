using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using System.Collections.Generic;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface ITemplatesService
    {
        IEnumerable<MetadataInfo> GetProjectTypes();

        IEnumerable<MetadataInfo> GetFrameworks(string projectType);

        IEnumerable<TemplateInfo> GetPages(string projectType, string frontEndFramework, string backEndFramework);

        IEnumerable<TemplateInfo> GetFeatures(string projectType, string frontEndFramework, string backEndFramework);
    }
}
