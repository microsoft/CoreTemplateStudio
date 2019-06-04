// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

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
    public class FeatureController : Controller
    {
        [EnableCors("AllowAll")]
        [HttpGet]
        public ActionResult<List<TemplateInfo>> GetFeaturesForFrameworks(string projectType, string frontEndFramework, string backEndFramework)
        {
            if (frontEndFramework == null && backEndFramework == null)
            {
                return BadRequest(new { message = StringRes.BadReqNoBackendOrFrontend });
            }

            var platform = GenContext.CurrentPlatform;
            var features = GenContext.ToolBox.Repo.GetTemplatesInfo(
                                                                TemplateType.Page,
                                                                platform,
                                                                projectType,
                                                                frontEndFramework,
                                                                backEndFramework);

            return features.ToList();
        }
    }
}
