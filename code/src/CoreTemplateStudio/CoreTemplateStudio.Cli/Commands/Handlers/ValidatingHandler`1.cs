// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class ValidatingHandler<T> : ICommandHandler<T>
    where T : ICommand
    {
        private readonly ICommandHandler<T> handler;
        private readonly IValidator<T> validator;

        public ValidatingHandler(ICommandHandler<T> handler, IValidator<T> validator)
        {
            this.handler = handler;
            this.validator = validator;
        }

        public async Task<int> ExecuteAsync(T command)
        {
            var validationResult = validator.Validate(command);

            if (validationResult.IsValid)
            {
                return await handler.ExecuteAsync(command);
            }

            throw new ValidationException(validationResult.Errors);
        }
    }
}
