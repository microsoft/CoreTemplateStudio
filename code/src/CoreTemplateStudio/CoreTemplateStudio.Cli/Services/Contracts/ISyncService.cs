using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Core.Locations;
using System;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface ISyncService
    {
        Task<SyncModel> ProcessAsync(string path, Action<SyncStatus, int> statusListener);
    }
}