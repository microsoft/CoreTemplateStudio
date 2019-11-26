// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentValidation;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GetLayoutsValidator : AbstractValidator<GetLayoutsCommand>
    {
        public GetLayoutsValidator()
        {
            RuleFor(x => GenContext.ToolBox)
                .NotEmpty()
                .WithMessage(StringRes.BadReqNotSynced);

            RuleFor(x => x.ProjectType)
                .NotEmpty()
                .WithMessage(StringRes.BadReqInvalidProjectType);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.FrontendFramework) || !string.IsNullOrEmpty(x.BackendFramework))
                .WithMessage(StringRes.BadReqNoBackendOrFrontend);
        }
    }
}
