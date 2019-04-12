// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.HubHandlers
{
    public class SyncHandler
    {
        private readonly string _platform;
        private readonly string _path;
        private readonly string _language;
        private readonly Action<SyncStatus, int> _statusListener;

        public SyncHandler(string platform, string path, string language, Action<SyncStatus, int> statusListener)
        {
            _platform = platform;
            _path = path;
            _language = language;
            _statusListener = statusListener;
        }

        public async Task<ActionResult<SyncModel>> AttemptSync()
        {
            if (!Platforms.IsValidPlatform(_platform))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidPlatform });
            }

            if (!IsValidPath(_path))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidPath });
            }

            if (!ProgrammingLanguages.IsValidLanguage(_language, _platform))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidLanguage });
            }

            SyncModel syncHelper = new SyncModel(_platform, _language, _path, _statusListener);

            await syncHelper.Sync();

            return syncHelper;
        }

        private bool IsValidPath(string path)
        {
            string suffix = string.Empty;
#if DEBUG
            suffix = "/templates";
#else
            suffix = $"{_platform}.{_language}.Templates.mstx";
#endif

            return path != null
                && suffix == "/templates" ? Directory.Exists(path + suffix)
                                          : File.Exists(Path.Combine(path, suffix));
        }
    }
}
