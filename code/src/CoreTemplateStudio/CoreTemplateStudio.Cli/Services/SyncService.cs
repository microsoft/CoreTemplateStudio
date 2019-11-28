// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Cli.Services
{
    public class SyncService : ISyncService
    {
        private readonly IMessageService _messageService;
        private bool _wasUpdated;

        public SyncService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<SyncModel> ProcessAsync(string path, string fullPath, string platform, string language)
        {
            try
            {
#if DEBUG
                GenContext.Bootstrap(
                    new LocalTemplatesSource(
                        path,
                        "0.0.0.0",
                        string.Empty),
                    new CliGenShell(_messageService),
                    new Version("0.0.0.0"),
                    platform,
                    language);
#else
                GenContext.Bootstrap(
                  new RemoteTemplatesSource(
                      platform,
                      language,
                      path,
                      new CliDigitalSignatureService()),
                  new CliGenShell(_messageService),
                  new Version(GetFileVersion()),
                  platform,
                  language);
#endif
                GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);

                return new SyncModel()
                {
                    TemplatesVersion = GenContext.ToolBox.TemplatesVersion,
                    WasUpdated = _wasUpdated,
                    ProjectNameValidationConfig = GenContext.ToolBox.Repo.ProjectNameValidationConfig,
                    ItemNameValidationConfig = GenContext.ToolBox.Repo.ItemNameValidationConfig,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(StringRes.ErrorSyncingTemplates, ex.Message));
            }
        }

        private static string GetFileVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }

        private void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            _messageService.SendResult(MessageType.SyncProgress, new { args.Status, args.Progress });

            if (args.Status.Equals(SyncStatus.Updated))
            {
                _wasUpdated = true;
            }
        }
    }
}
