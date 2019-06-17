using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Services
{
    public class GetFrameworksService : IGetFrameworksService
    {
        private readonly IMessageService _messageService;

        public GetFrameworksService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<int> ProcessAsync(GetFrameworksOptions options)
        {
            var projectTypes = GetFrameworks(options.ProjectType);
            _messageService.Send(projectTypes);

            return await Task.FromResult(0);
        }

        private IEnumerable<MetadataInfo> GetFrameworks(string projectType)
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
    }
}
