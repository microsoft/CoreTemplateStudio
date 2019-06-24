// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentValidation;
using Microsoft.Templates.Cli.Resources;

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
