using Microsoft.Templates.Cli.Resources;
using FluentValidation;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetFeaturesValidator : GenContextValidator<GetFeaturesCommand>
    {
        public GetFeaturesValidator()
        {
            RuleFor(x => x)
                 .Must(x => x.FrontendFramework != null || x.BackendFramework != null)
                 .WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }        
    }
}
