using Microsoft.Templates.Cli.Commands.Validators;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface IValidateHandler<T> where T : ICommand
    {
        Task<CommandValidatorResult> ValidateAsync(T command);
    }
}
