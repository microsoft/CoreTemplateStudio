using Microsoft.Templates.Cli.Commands.Contracts;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class CloseHandler : ICommandHandler<CloseCommand>
    {
        public async Task<int> ExecuteAsync(CloseCommand command)
        {
            // return 1 to close the Cli
            return await Task.FromResult(1);
        }
    }
}
