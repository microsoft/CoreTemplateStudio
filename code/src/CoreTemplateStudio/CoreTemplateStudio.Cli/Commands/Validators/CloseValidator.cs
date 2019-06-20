using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class CloseValidator : ICommandValidator<CloseCommand>
    {
        public CommandValidatorResult Validate(CloseCommand command)
        {
            return new CommandValidatorResult();
        }
    }
}
