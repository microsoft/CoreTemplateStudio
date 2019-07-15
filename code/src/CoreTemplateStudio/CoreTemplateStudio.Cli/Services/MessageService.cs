// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

        public void SendResult<T>(MessageType type, T item)
        {
            var result = new { MessageType = type, Content = item };

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

            var json = JsonConvert.SerializeObject(result, settings);

            SendMessage(json);
        }
    }
}
