using Microsoft.Templates.Cli.Resources;
using FluentValidation;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetFeaturesValidator : GenContextValidator<GetFeaturesCommand>
    {
        public GetFeaturesValidator()
        {
            RuleFor(x => x.FrontendFramework)
                .NotEmpty()
                .DependentRules(() => {
                    RuleFor(x => x.BackendFramework).NotEmpty();
            }).WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }        
    }
}
