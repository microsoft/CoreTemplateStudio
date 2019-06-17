using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetPagesHandler : ICommandHandler<GetPagesCommand>
    {
        private readonly ITemplatesService _templatesService;

        public GetPagesHandler(ITemplatesService templatesService)
        {
            _templatesService = templatesService;
        }

        public async Task<int> ExecuteAsync(GetPagesCommand command)
        {
            return await Task.FromResult(0);
        }
    }
}
