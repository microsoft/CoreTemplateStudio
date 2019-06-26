// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentValidation;
using FluentValidation.Results;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class GenContextValidator<T> : AbstractValidator<T>
    {
        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (GenContext.ToolBox == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(GenContext.ToolBox), StringRes.BadReqNotSynced));
                return false;
            }

            return true;
        }
    }
}
