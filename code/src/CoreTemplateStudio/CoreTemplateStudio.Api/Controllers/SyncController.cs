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
        [HttpPost]
        public async Task<ActionResult<SyncModel>> SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            if (!Platforms.IsValidPlatform(platform))
            {
                return BadRequest(new { message = "invalid platform" });
            }

            if (!IsValidPath(path))
            {
                return BadRequest(new { message = "invalid path" });
            }

            if (!ProgrammingLanguages.IsValidLanguage(language, platform))
            {
                return BadRequest(new { message = "invalid language for this platform." });
            }

            SyncModel syncHelper = new SyncModel(platform, language, path);

            await syncHelper.Sync();

            return syncHelper;
        }

        public bool IsValidPath(string path)
        {
            string suffix = string.Empty;
#if DEBUG
            suffix = "/templates";
#else
            suffix = $"{_platform}.{_language}.Templates.mstx";
#endif
            return path != null
                && suffix == "/templates" ? Directory.Exists(path + suffix)
                                          : System.IO.File.Exists(path + suffix);
        }
    }
}
