using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
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
            var Services = _templatesService.GetServices(command.ProjectType, command.FrontendFramework, command.BackendFramework);
            _messageService.Send(Services);

            return await Task.FromResult(0);
        }
    }
}
