using Microsoft.Templates.Cli.Commands.Validators;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface ICommandValidator<T> where T : ICommand
    {
        CommandValidatorResult Validate(T command);
    }
}
