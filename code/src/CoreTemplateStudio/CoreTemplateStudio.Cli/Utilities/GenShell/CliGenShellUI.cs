// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Cli.Utilities.GenShell
{
    public class CliGenShellUI : IGenShellUI
    {
        private readonly IMessageService _messageService;

        public CliGenShellUI(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void CancelWizard(bool back = true)
        {
        }

        public void OpenItems(params string[] itemsFullPath)
        {
        }

        public void OpenProjectOverview()
        {
        }

        public void ShowModal(IWindow shell)
        {
        }

        public void ShowStatusBarMessage(string message)
        {
            _messageService.SendResult(MessageType.GenerateProgress, message);
        }

        public void ShowTaskList()
        {
        }

        public void WriteOutput(string data)
        {
        }
    }
}
