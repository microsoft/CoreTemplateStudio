// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.Models
{
    // just a base implementation for the API scaffolding,
    // can't provide working logic until the engine is done.
    public class SyncModel
    {
        private readonly string _platform;
        private readonly string _language;
        private readonly string _installedPackagePath;

        public bool WasUpdated { get; set; }

        public SyncModel(string platform, string language, string installedPackagePath)
        {
            _platform = platform;
            _language = language;
            _installedPackagePath = installedPackagePath;
        }

        public void Sync()
        {
#if DEBUG
            GenContext.Bootstrap(
                new LocalTemplatesSource(
                    _installedPackagePath,
                    "1.0.0.0",
                    string.Empty),
                new ApiShell(),
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
                new ApiShell(),
                new Version(1, 0, 0, 0),
                _platform,
                _language);
#endif
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
            GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
        }

        public bool IsValidLanguage()
        {
            // TODO: Validity hard coded for now but will be updated.
            bool isValid = false;

            // Validate that the language inputted is valid.
            foreach (string lang in ProgrammingLanguages.GetAllLanguages())
            {
                if (lang.Equals(_language, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            bool isUwpInvalidLanguage = _language != null ? _platform.Equals(Platforms.Uwp, StringComparison.OrdinalIgnoreCase)
                                        && _language.Equals(ProgrammingLanguages.Any, StringComparison.OrdinalIgnoreCase)
                                        : true;

            isValid &= !isUwpInvalidLanguage;

            return isValid;
        }

        public bool IsValidPlatform()
        {
            bool isValid = false;

            foreach (string plat in Platforms.GetAllPlatforms())
            {
                if (plat.Equals(_platform, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public bool IsValidPath()
        {
            return _installedPackagePath != null
                && Directory.Exists(_installedPackagePath);
        }

        private void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            if (args.Status.Equals(SyncStatus.Updated))
            {
                WasUpdated = true;
            }
        }
    }
}
