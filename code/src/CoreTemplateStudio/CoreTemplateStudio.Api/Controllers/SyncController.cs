// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : Controller
    {
        // POST: api/sync?platform={platform}&path={path}&language={language}
        // platform is required (Web for WebTS and Uwp for WinTS), returns a 400 bad request otherwise

        /// <summary>
        /// POST: api/sync?platform=<>&path=<>&language=<>
        /// API Endpoint to potentially sync templates used in generation with the cdn or local visx if release or templates folder
        /// if debug for updates.
        /// </summary>
        /// <param name="platform">The platform the caller is generating for. Supports Uwp and Web for now.</param>
        /// <param name="path">The path to the vsix or the template folder</param>
        /// <param name="language">The language, which should be one of the ones defined in the engine.</param>
        /// <returns>JSON result with whether there was a sync or not, and a message detailing error if failed response</returns>
        [HttpPost]
        public async Task<JsonResult> SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            if (!Platforms.IsValidPlatform(platform))
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }
            else if (!IsValidPath(path))
            {
                return Json(BadRequest(new { message = "invalid path" }));
            }
            else if (!ProgrammingLanguages.IsValidLanguage(language, platform))
            {
                return Json(BadRequest(new { message = "invalid language for this platform." }));
            }

            SyncModel syncHelper = new SyncModel(platform, language, path);

            await syncHelper.Sync();

            return Json(Ok(syncHelper));
        }

        public bool IsValidPath(string path)
        {
            string suffix = string.Empty;
#if DEBUG
            suffix = "/Templates";
#else
            suffix = $"{_platform}.{_language}.Templates.mstx";
#endif
            return path != null
                && suffix == "/Templates" ? Directory.Exists(path + suffix)
                                          : System.IO.File.Exists(path + suffix);
        }
    }
}
