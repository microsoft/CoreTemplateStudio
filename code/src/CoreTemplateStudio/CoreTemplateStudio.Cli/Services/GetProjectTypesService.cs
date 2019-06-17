using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Newtonsoft.Json;

namespace Microsoft.Templates.Cli.Services
{
    public class GetProjectTypesService : IGetProjectTypesService
    {
        private readonly IMessageService _messageService;

        public GetProjectTypesService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<int> ProcessAsync(GetProjectTypesOptions options)
        {
            var projectTypes = GetProjectTypes();
            _messageService.Send(projectTypes);

            return await Task.FromResult(0);
        }

        public IEnumerable<MetadataInfo> GetProjectTypes()
        {
            var platform = GenContext.CurrentPlatform;
            var result = GenContext.ToolBox.Repo.GetProjectTypes(platform);

            return result;            
        }
    }
}