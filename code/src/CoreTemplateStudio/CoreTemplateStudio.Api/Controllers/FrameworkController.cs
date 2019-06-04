// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using CoreTemplateStudio.Api.Extensions.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateGenContextFilter]
    public class FrameworkController : Controller
    {
        [EnableCors("AllowAll")]
        [HttpGet]
        public ActionResult<List<MetadataInfo>> GetFrameworks(string projectType)
        {
            if (projectType == null)
            {
                return BadRequest(new { message = StringRes.BadReqInvalidProjectType });
            }

            var platform = GenContext.CurrentPlatform;
            var frontendFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType);
            var backendFrameworks = GenContext.ToolBox.Repo.GetBackEndFrameworks(platform, projectType);

            var targetFrameworks = new List<MetadataInfo>();
            targetFrameworks.AddRange(frontendFrameworks);
            targetFrameworks.AddRange(backendFrameworks);

            return targetFrameworks;
        }
    }
}
