using Microsoft.Templates.Cli.Resources;
using FluentValidation;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetTestingsValidator : GenContextValidator<GetTestingsCommand>
    {
        public GetTestingsValidator()
        {
            RuleFor(x => x)
                .Must(x => x.FrontendFramework != null || x.BackendFramework != null)
                .WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }
    }
}
