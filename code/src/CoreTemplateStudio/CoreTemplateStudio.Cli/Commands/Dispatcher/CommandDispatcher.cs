using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands.Contracts;
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

        public async Task<int> DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command can not be null.");
            }

            var handler = _serviceProvider.GetService<ICommandHandler<T>>();
            return await handler.ExecuteAsync(command);
        }
    }
}
