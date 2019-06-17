using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            //todo: validate and test
            var result = await _syncService.ProcessAsync(command.Path, SyncStatusChanged);
            _messageService.Send(result);
            return 0;
        }

        private void SyncStatusChanged(SyncStatus status, int progress)
        {
            _messageService.SendMessage($"syncMessage : {status.ToString()} - {progress}");

        }
    }
}
