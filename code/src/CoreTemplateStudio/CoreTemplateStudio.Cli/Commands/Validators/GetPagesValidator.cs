using Microsoft.Templates.Cli.Resources;
using FluentValidation;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetPagesValidator : GenContextValidator<GetPagesCommand>
    {
        public GetPagesValidator()
        {
            RuleFor(x => x.FrontendFramework)
                .NotEmpty()
                .DependentRules(() => {
                    RuleFor(x => x.BackendFramework).Empty();
                }).WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }
    }
}
