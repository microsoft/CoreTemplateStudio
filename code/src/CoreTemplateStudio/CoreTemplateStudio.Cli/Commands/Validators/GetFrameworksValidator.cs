using Microsoft.Templates.Cli.Resources;
using FluentValidation;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetFrameworksValidator : GenContextValidator<GetFrameworksCommand>
    {
        public GetFrameworksValidator()
        {
            RuleFor(x => x.ProjectType)
                .NotEmpty()
                .WithMessage(StringRes.BadReqInvalidProjectType);
        }
    }
}
