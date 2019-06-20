using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetFrameworksValidator : ICommandValidator<GetFrameworksCommand>
    {
        public CommandValidatorResult Validate(GetFrameworksCommand command)
        {
            var validationResult = new CommandValidatorResult();

            if (GenContext.ToolBox == null)
            {
                validationResult.AddMessage(StringRes.BadReqNotSynced);
            }

            if (string.IsNullOrEmpty(command.ProjectType))
            {
                validationResult.AddMessage(StringRes.BadReqInvalidProjectType);
            }

            return validationResult;
        }
    }
}
