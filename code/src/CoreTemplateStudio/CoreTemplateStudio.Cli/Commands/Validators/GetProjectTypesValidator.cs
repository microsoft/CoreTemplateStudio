using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetProjectTypesValidator : ICommandValidator<GetProjectTypesCommand>
    {
        public CommandValidatorResult Validate(GetProjectTypesCommand command)
        {
            var validationResult = new CommandValidatorResult();

            if (GenContext.ToolBox == null)
            {
                validationResult.Messages.Add(StringRes.BadReqNotSynced);
            }

            return validationResult;
        }
    }
}
