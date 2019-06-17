using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task<int> ExecuteAsync(T command);
    }
}
