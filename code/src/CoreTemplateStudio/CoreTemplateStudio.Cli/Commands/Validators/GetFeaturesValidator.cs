using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetFeaturesValidator : ICommandValidator<GetFeaturesCommand>
    {
        public CommandValidatorResult Validate(GetFeaturesCommand command)
        {
            var validationResult = new CommandValidatorResult();

            if (GenContext.ToolBox == null)
            {
                validationResult.AddMessage(StringRes.BadReqNotSynced);
            }

            if (string.IsNullOrEmpty(command.FrontendFramework) && string.IsNullOrEmpty(command.BackendFramework))
            {
                validationResult.AddMessage(StringRes.BadReqNoBackendOrFrontend);
            }

            return validationResult;
        }
    }
}
