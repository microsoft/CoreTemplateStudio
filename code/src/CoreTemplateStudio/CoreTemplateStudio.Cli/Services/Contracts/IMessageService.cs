using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IMessageService
    {
        void SendMessage(string message);

        void SendError(string error);

        void Send<T>(T item);
    }
}
