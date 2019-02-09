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
        // POST: api/sync/{platform}
        // platform is required (Web for WebTS and Uwp for WinTS), returns a 400 bad request otherwise
        [HttpPost]
        public JsonResult SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            if (!IsValidPlatform(platform))
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }
            else if (!IsValidPath(path))
            {
                return Json(BadRequest(new { message = "invalid path" }));
            }
            else if (!IsValidLanguage(platform, language))
            {
                return Json(BadRequest(new { message = "invalid language for this platform." }));
            }

            SyncModel syncHelper = new SyncModel(platform, language, path);
            syncHelper.Sync();

            return Json(Ok(syncHelper));
        }

        private bool IsValidLanguage(string platform, string language)
        {
            // TODO: Validity hard coded for now but will be updated.
            bool isValid = false;

            // Validate that the language inputted is valid.
            foreach (string lang in ProgrammingLanguages.GetAllLanguages())
            {
                if (lang.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            bool isUwpInvalidLanguage = language != null ? platform.Equals(Platforms.Uwp, StringComparison.OrdinalIgnoreCase)
                                        && language.Equals(ProgrammingLanguages.Any, StringComparison.OrdinalIgnoreCase)
                                        : true;

            isValid &= !isUwpInvalidLanguage;

            return isValid;
        }

        private bool IsValidPlatform(string platform)
        {
            bool isValid = false;

            foreach (string plat in Platforms.GetAllPlatforms())
            {
                if (platform.Equals(plat, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private bool IsValidPath(string path)
        {
            return path != null
                && Directory.Exists(path);
        }
    }
}
