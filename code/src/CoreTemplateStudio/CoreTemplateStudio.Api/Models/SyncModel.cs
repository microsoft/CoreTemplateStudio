// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.Models
{
    public class SyncModel
    {
        private readonly string _platform;
        private readonly string _language;
        private readonly string _installedPackagePath;
        private readonly Action<SyncStatus, int> _callback;

        public bool WasUpdated { get; set; }

        public SyncModel(string platform, string language, string installedPackagePath, Action<SyncStatus, int> callback)
        {
            _platform = platform;
            _language = language;
            _installedPackagePath = installedPackagePath;
            _callback = callback;
        }

        public async Task Sync()
        {
#if DEBUG
            GenContext.Bootstrap(
                new LocalTemplatesSource(
                    _installedPackagePath,
                    "1.0.0.0",
                    string.Empty),
                new ApiGenShell(),
                new Version(1, 0, 0, 0),
                _platform,
                _language);
#else
            GenContext.Bootstrap(
                new RemoteTemplatesSource(
                    _platform,
                    _language,
                    _installedPackagePath,
                    new ApiDigitalSignatureService()),
                new ApiGenShell(),
                new Version(1, 0, 0, 0),
                _platform,
                _language);
#endif
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
            await GenContext.ToolBox.Repo.SynchronizeAsync(true);
        }

        private void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            _callback.Invoke(args.Status, args.Progress);

            if (args.Status.Equals(SyncStatus.Updated))
            {
                WasUpdated = true;
            }
        }
    }
}
