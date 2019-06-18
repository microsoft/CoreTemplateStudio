using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetFrameworksHandler : ICommandHandler<GetFrameworksCommand>
    {
        private readonly IMessageService _messageService;
        private readonly ITemplatesService _templatesService;

        public GetFrameworksHandler(IMessageService messageService, ITemplatesService templatesService)
        {
            _messageService = messageService;
            _templatesService = templatesService;
        }

        public async Task<int> ExecuteAsync(GetFrameworksCommand command)
        {
            var frameworks = _templatesService.GetFrameworks(command.ProjectType);
            _messageService.Send(frameworks);

            return await Task.FromResult(0);
        }
    }
}
