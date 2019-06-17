using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Commands.Contracts
{
    public interface ICommandHandler<T> where T : ICommand
    {
        int Execute(T command);
    }
}
