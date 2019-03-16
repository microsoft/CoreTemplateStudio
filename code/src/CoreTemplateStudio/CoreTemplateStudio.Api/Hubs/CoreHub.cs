// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.Hubs
{
    public class CoreHub : Hub
    {
        public async Task<ActionResult<SyncModel>> SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            if (!Platforms.IsValidPlatform(platform))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidPlatform });
            }

            if (!IsValidPath(path))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidPath });
            }

            if (!ProgrammingLanguages.IsValidLanguage(language, platform))
            {
                return new BadRequestObjectResult(new { message = StringRes.BadReqInvalidLanguage });
            }

            SyncModel syncHelper = new SyncModel(platform, language, path, SendMessageToClient);

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
                                          : File.Exists(path + suffix);
        }

        private void SendMessageToClient(SyncStatus status)
        {
            Clients.Caller.SendAsync("syncMessage", status);
        }
    }
}
