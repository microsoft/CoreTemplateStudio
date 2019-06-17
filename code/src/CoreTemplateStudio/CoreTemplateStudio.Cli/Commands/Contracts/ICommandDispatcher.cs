using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface ICommandDispatcher
    {
        Task<int> DispatchAsync<T>(T command) where T : ICommand;
    }
}
