// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.HubHandlers
{
    public class SyncHandler
    {
        private readonly string _platform;
        private readonly string _path;
        private readonly string _fullPath;
        private readonly string _language;
        private readonly Action<SyncStatus, int> _statusListener;
        private bool _wasUpdated;

        public SyncHandler(string platform, string path, string language, Action<SyncStatus, int> statusListener)
        {
            _platform = "Web";
            _language = "Any";

#if DEBUG
            _path = path;
            _fullPath = path + "/templates";

#else
            _path = @"..";
            _fullPath = Path.Combine(_path, $"{_platform}.{_language}.Templates.mstx");
#endif
            _statusListener = statusListener;
        }

        public async Task<ActionResult<SyncModel>> Sync()
        {
            if (!Platforms.IsValidPlatform(_platform))
            {
                throw new HubException(StringRes.BadReqInvalidPlatform);
            }

            if (!IsValidPath(_fullPath))
            {
                throw new HubException(StringRes.BadReqInvalidPath);
            }

            if (_fullPath.EndsWith("mstx") && !IsPackageValid(_fullPath))
            {
                throw new HubException(StringRes.BadReqInvalidPackage);
            }

            if (!ProgrammingLanguages.IsValidLanguage(_language, _platform))
            {
                throw new HubException(StringRes.BadReqInvalidLanguage);
            }

            try
            {
#if DEBUG
                GenContext.Bootstrap(
                    new LocalTemplatesSource(
                        _path,
                        "0.0.0.0",
                        string.Empty),
                    new ApiGenShell(),
                    new Version("0.0.0.0"),
                    _platform,
                    _language);
#else
                GenContext.Bootstrap(
                  new RemoteTemplatesSource(
                      _platform,
                      _language,
                      _path,
                      new ApiDigitalSignatureService()),
                  new ApiGenShell(),
                  new Version("1.0.0.0"),
                  _platform,
                  _language);
#endif
                GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);
                return new SyncModel()
                {
                    TemplatesVersion = GenContext.ToolBox.TemplatesVersion,
                    WasUpdated = _wasUpdated,
                };
            }
            catch (Exception ex)
            {
                throw new HubException(string.Format(StringRes.ErrorSyncingTemplates, ex.Message));
            }
        }

        private bool IsPackageValid(string fullpath)
        {
            return Configuration.Current.AllowedPackages.Contains(GetHash(fullpath));
        }

        private string GetHash(string fullpath)
        {
            using (FileStream stream = File.OpenRead(fullpath))
            {
                var sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        private void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            _statusListener.Invoke(args.Status, args.Progress);

            if (args.Status.Equals(SyncStatus.Updated))
            {
                _wasUpdated = true;
            }
        }

        private bool IsValidPath(string fullpath)
        {
#if DEBUG
            return fullpath != null && Directory.Exists(fullpath);
#else
            return fullpath != null && File.Exists(fullpath);
#endif
        }
    }
}
