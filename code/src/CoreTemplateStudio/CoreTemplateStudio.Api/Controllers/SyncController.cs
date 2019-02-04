// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CoreTemplateStudio.Api.Enumerables;
using CoreTemplateStudio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : Controller
    {
        // POST: api/sync/{platform}
        // platform is required (Web for WebTS and Uwp for WinTS), returns a 400 bad request otherwis
        [HttpPost("{platform}")]
        public JsonResult SyncTemplates(string platform)
        {
            // actual logic to be written once CDN is setup and the engine supports
            // syncing. This is just scaffolding to get started
            if (!platform.Equals("uwp", StringComparison.OrdinalIgnoreCase) && !platform.Equals("web", StringComparison.OrdinalIgnoreCase))
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }

            Enum.TryParse(platform, out Platform pform);

            SyncItem syncHelper = new SyncItem(pform);
            syncHelper.Sync();

            return Json(Ok(syncHelper));
        }
    }
}
