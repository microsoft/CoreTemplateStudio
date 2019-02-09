// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : Controller
    {
        // POST: api/sync?platform={platform}&path={path}&language={language}
        // platform is required (Web for WebTS and Uwp for WinTS), returns a 400 bad request otherwise
        [HttpPost]
        public JsonResult SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            SyncModel syncHelper = new SyncModel(platform, language, path);

            if (!syncHelper.IsValidPlatform())
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }
            else if (!syncHelper.IsValidPath())
            {
                return Json(BadRequest(new { message = "invalid path" }));
            }
            else if (!syncHelper.IsValidLanguage())
            {
                return Json(BadRequest(new { message = "invalid language for this platform." }));
            }

            syncHelper.Sync();

            return Json(Ok(syncHelper));
        }
    }
}
