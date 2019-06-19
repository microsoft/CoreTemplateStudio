using Microsoft.Templates.Cli.Commands.Validators;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface ICommandDispatcher
    {
        Task<int> DispatchAsync<T>(T command) where T : ICommand;

        Task<CommandValidatorResult> ValidateAsync<T>(T command) where T : ICommand;
    }
}
