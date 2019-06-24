// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Templates.Cli.Services.Contracts;
using Newtonsoft.Json;

namespace Microsoft.Templates.Cli.Services
{
    public class MessageService : IMessageService
    {
        public void SendError(string error)
        {
            Console.Error.WriteLine(error);
        }

        public void SendErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                SendError(error);
            }
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Send<T>(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            SendMessage(json);
        }
    }
}
