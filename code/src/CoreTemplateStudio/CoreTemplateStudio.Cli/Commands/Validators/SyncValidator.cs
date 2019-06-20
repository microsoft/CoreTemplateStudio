using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class SyncValidator : ICommandValidator<SyncCommand>
    {
        public CommandValidatorResult Validate(SyncCommand command)
        {
            var validationResult = new CommandValidatorResult();
            
            if (string.IsNullOrEmpty(command.Path))
            {
                validationResult.AddMessage(StringRes.BadReqInvalidPath);
            }

            return validationResult;
        }
    }
}
