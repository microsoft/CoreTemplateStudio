using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Commands.Validators;
using System;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Dispatcher
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //todo : refactor exceptions
        public async Task<int> DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command can not be null.");
            }

            var handler = _serviceProvider.GetService<ICommandHandler<T>>();


            if (handler == null)
            {
                throw new Exception($"{command.GetType().Name} handler was not found.");
            }

            return await handler.ExecuteAsync(command);
        }

        public CommandValidatorResult Validate<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command can not be null.");
            }
            var services =_serviceProvider.GetServices<ICommandHandler<ICommand>>();

            var handler = _serviceProvider.GetService<ICommandValidator<T>>();

            if (handler == null)
            {
                throw new Exception($"{command.GetType().Name} validator was not found.");
            }

            return handler.Validate(command);
        }
    }
}
