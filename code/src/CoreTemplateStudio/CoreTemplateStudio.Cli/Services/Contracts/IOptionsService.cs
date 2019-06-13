using Microsoft.Templates.Cli.Options;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IOptionsService<T> where T : IOptions
    {
        int Process(T options);
    }
}