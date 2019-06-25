// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class SyncHandler : ICommandHandler<SyncCommand>
    {
        private readonly IMessageService _messageService;
        private readonly ISyncService _syncService;

        public SyncHandler(IMessageService messageService, ISyncService syncService)
        {
            _messageService = messageService;
            _syncService = syncService;
        }

        public async Task<int> ExecuteAsync(SyncCommand command)
        {
            var result = await _syncService.ProcessAsync(
                                                        command.Path,
                                                        command.FullPath,
                                                        command.Platform,
                                                        command.Language,
                                                        SyncStatusChanged);
            _messageService.Send(result);
            return 0;
        }

        private void SyncStatusChanged(SyncStatus status, int progress)
        {
            _messageService.SendMessage($"syncMessage : {status.ToString()} - {progress}");
        }
    }
}
