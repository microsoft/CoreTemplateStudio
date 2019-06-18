using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetProjectTypesHandler : ICommandHandler<GetProjectTypesCommand>
    {
        private readonly IMessageService _messageService;
        private readonly ITemplatesService _templatesService;

        public GetProjectTypesHandler(IMessageService messageService, ITemplatesService templatesService)
        {
            _messageService = messageService;
            _templatesService = templatesService;
        }

        public async Task<int> ExecuteAsync(GetProjectTypesCommand command)
        {
            var projectTypes = _templatesService.GetProjectTypes();
            _messageService.Send(projectTypes);

            return await Task.FromResult(0);

        }
    }
}
