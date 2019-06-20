using FluentValidation;
using Microsoft.Templates.Cli.Resources;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GenerateValidator : GenContextValidator<GenerateCommand>
    {
        public GenerateValidator()
        {
            RuleFor(x => x.GenerationDataJson)
                .NotEmpty()
                .WithMessage(StringRes.BadReqInvalidGenJson);
        }
    }
}
