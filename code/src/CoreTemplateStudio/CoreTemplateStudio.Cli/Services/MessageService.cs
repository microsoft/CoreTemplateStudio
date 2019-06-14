using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli.Services
{
    class MessageService : IMessageService
    {
        public void SendError(string error)
        {
            Console.WriteLine(error);
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
