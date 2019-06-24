// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Cli.Services
{
    public class SyncService : ISyncService
    {
        private readonly string _platform = "Web";
        private readonly string _language = "Any";
        private string _path;
        private string _fullPath;
        private Action<SyncStatus, int> _statusListener;
        private bool _wasUpdated;

        public async Task<SyncModel> ProcessAsync(string path, Action<SyncStatus, int> statusListener)
        {
            ConfigurePaths(path);
            _statusListener = statusListener;

            if (!Platforms.IsValidPlatform(_platform))
            {
                throw new Exception(StringRes.BadReqInvalidPlatform);
            }

            if (!IsValidPath(_fullPath))
            {
                throw new Exception(StringRes.BadReqInvalidPath);
            }

            if (_fullPath.EndsWith("mstx") && !IsPackageValid(_fullPath))
            {
                throw new Exception(StringRes.BadReqInvalidPackage);
            }

            if (!ProgrammingLanguages.IsValidLanguage(_language, _platform))
            {
                throw new Exception(StringRes.BadReqInvalidLanguage);
            }

            try
            {
#if DEBUG
                GenContext.Bootstrap(
                    new LocalTemplatesSource(
                        _path,
                        "0.0.0.0",
                        string.Empty),
                    new CliGenShell(),
                    new Version("0.0.0.0"),
                    _platform,
                    _language);
#else
                GenContext.Bootstrap(
                  new RemoteTemplatesSource(
                      _platform,
                      _language,
                      _path,
                      new CliDigitalSignatureService()),
                  new CliGenShell(),
                  new Version(GetFileVersion()),
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
                throw new Exception(string.Format(StringRes.ErrorSyncingTemplates, ex.Message));
            }
        }

        private static string GetFileVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
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

        private void ConfigurePaths(string path)
        {
#if DEBUG
            _path = path;
            _fullPath = path + "/templates";

#else
            _path = @"..";
            _fullPath = Path.Combine(_path, $"{_platform}.{_language}.Templates.mstx");
#endif
        }
    }
}
