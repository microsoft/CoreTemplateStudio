using Microsoft.Templates.Cli.Resources;
using FluentValidation;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetPagesValidator : GenContextValidator<GetPagesCommand>
    {
        public GetPagesValidator()
        {
            RuleFor(x => x)
                .Must(x => x.FrontendFramework != null || x.BackendFramework != null)
                .WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }
    }
}
