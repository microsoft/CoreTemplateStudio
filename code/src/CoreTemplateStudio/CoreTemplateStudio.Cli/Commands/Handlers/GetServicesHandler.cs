// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetServicesHandler : ICommandHandler<GetServicesCommand>
    {
        private readonly IMessageService _messageService;
        private readonly ITemplatesService _templatesService;

        public GetServicesHandler(IMessageService messageService, ITemplatesService templatesService)
        {
            _messageService = messageService;
            _templatesService = templatesService;
        }

        public async Task<int> ExecuteAsync(GetServicesCommand command)
        {
            var services = _templatesService.GetServices(command.ProjectType, command.FrontendFramework, command.BackendFramework);
            _messageService.SendResult(MessageType.GetServicesResult, services);

            return await Task.FromResult(0);
        }
    }
}
