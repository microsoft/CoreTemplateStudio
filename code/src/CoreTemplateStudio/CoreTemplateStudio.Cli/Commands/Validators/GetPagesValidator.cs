using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;
namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetPagesValidator : ICommandValidator<GetPagesCommand>
    {
        public CommandValidatorResult Validate(GetPagesCommand command)
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
