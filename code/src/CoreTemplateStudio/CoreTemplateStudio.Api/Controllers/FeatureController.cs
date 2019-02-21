// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : Controller
    {
        /// <summary>
        /// GET api/feature?projectType=<>&frontEndFramework=<>&backEndFramework
        /// Given a project type, and possibly either a frontend and backend framework
        /// will return a list of features supported by these filters.
        /// </summary>
        /// <param name="projectType">Project Type that was selected by the user</param>
        /// <param name="frontEndFramework">The frontend framework that was selected by the user</param>
        /// <param name="backEndFramework">The backend framework that was selected by the user</param>
        /// <returns>A list of features that match with the filters given.</returns>
        [HttpGet]
        public JsonResult GetFeaturesForFrameworks(string projectType, string frontEndFramework, string backEndFramework)
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            if (frontEndFramework == null && backEndFramework == null)
            {
                return Json(BadRequest(new { message = "You must specify a backend or frontend framework at the very least" }));
            }

            string platform = GenContext.CurrentPlatform;

            IEnumerable<ITemplateInfo> templates = GenComposer.GetFeatures(
                                                                           projectType,
                                                                           platform,
                                                                           frontEndFramework,
                                                                           backEndFramework);

            return Json(Ok(new { items = templates }));
        }
    }
}
