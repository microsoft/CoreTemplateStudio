using FluentValidation;
using Microsoft.Templates.Cli.Resources;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class SyncValidator : AbstractValidator<SyncCommand>
    {
        public SyncValidator()
        {
            RuleFor(c => c.Path)
                .NotEmpty()
                .WithMessage(StringRes.BadReqInvalidPath);
        }
    }
}
