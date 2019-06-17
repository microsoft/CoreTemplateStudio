using Microsoft.Templates.Cli.Commands.Contracts;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IOptionsService<T> where T : ICommand
    {
        Task<int> ProcessAsync(T options);
    }
}