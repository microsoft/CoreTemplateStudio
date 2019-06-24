using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetTestingsHandler : ICommandHandler<GetTestingsCommand>
    {
        private readonly IMessageService _messageService;
        private readonly ITemplatesService _templatesService;

        public GetTestingsHandler(IMessageService messageService, ITemplatesService templatesService)
        {
            _messageService = messageService;
            _templatesService = templatesService;
        }

        public async Task<int> ExecuteAsync(GetTestingsCommand command)
        {
            var Testing = _templatesService.GetTestings(command.ProjectType, command.FrontendFramework, command.BackendFramework);
            _messageService.Send(Testing);

            return await Task.FromResult(0);
        }
    }
}
