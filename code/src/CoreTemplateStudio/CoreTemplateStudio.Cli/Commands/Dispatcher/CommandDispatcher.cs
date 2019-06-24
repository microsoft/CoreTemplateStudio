// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Commands.Handlers;

namespace Microsoft.Templates.Cli.Commands.Dispatcher
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> DispatchAsync<T>(T command)
            where T : ICommand
        {
            var handler = _serviceProvider.GetService<ICommandHandler<T>>();
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator != null)
            {
                var validationHandler = new ValidatingHandler<T>(handler, validator);
                return await validationHandler.ExecuteAsync(command);
            }

            return await handler.ExecuteAsync(command);
        }
    }
}
