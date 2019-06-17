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

        public int Execute(GetPagesCommand command)
        {
            // do something here
            return 0;
        }
    }
}
