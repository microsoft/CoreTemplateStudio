using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GenerateValidator : ICommandValidator<GenerateCommand>
    {
        public CommandValidatorResult Validate(GenerateCommand command)
        {
            var validationResult = new CommandValidatorResult();
            if (GenContext.ToolBox == null)
            {
               validationResult.AddMessage(StringRes.BadReqNotSynced);
            }

            return validationResult;
        }
    }
}
