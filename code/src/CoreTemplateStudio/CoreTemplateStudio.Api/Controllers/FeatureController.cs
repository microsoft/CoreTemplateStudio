// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using CoreTemplateStudio.Api.Extensions.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateGenContextFilter]
    public class FeatureController : Controller
    {
        [HttpGet]
        public ActionResult<List<ITemplateInfo>> GetFeaturesForFrameworks(string projectType, string frontEndFramework, string backEndFramework)
        {
            if (frontEndFramework == null && backEndFramework == null)
            {
                return BadRequest(new { message = "You must specify a backend or frontend framework at the very least" });
            }

            string platform = GenContext.CurrentPlatform;

            var templates = GenComposer.GetFeatures(
                                                    projectType,
                                                    platform,
                                                    frontEndFramework,
                                                    backEndFramework);

            return templates.ToList();
        }
    }
}
