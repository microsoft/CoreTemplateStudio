using Microsoft.Templates.Cli.Options;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IOptionsService<T> where T : IOptions
    {
        Task<int> ProcessAsync(T options);
    }
}