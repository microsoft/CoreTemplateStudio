// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworkController : Controller
    {
        /// <summary>
        /// GET: api/framework
        /// Gets all frameworks available for the current platform and language.
        /// </summary>
        /// <returns>all frameworks for the current platform and language.</returns>
        [HttpGet]
        public JsonResult GetFrameworks()
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            var result = GenContext.ToolBox.Repo.GetFrameworks(GenContext.CurrentPlatform);
            return Json(Ok(new { items = result }));
        }

        /// <summary>
        /// GET: api/framework/{frameworkType}
        /// Gets all frameworks available for the current platform and language for the framework type specified.
        /// </summary>
        /// <param name="frameworkType">The type of framework we would like.</param>
        /// <returns>all frameworks for the current platform, framework type, and language.</returns>
        [HttpGet("{frameworkType}")]
        public JsonResult GetFrameworksOfType(string frameworkType)
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            if (!frameworkType.Equals("frontend", StringComparison.OrdinalIgnoreCase) && !frameworkType.Equals("backend", StringComparison.OrdinalIgnoreCase) && !frameworkType.Equals("uwpdesign", StringComparison.OrdinalIgnoreCase))
            {
                return Json(BadRequest(new { message = "frameworkType can only be frontend/backend/uwpdesign" }));
            }

            var type = new KeyValuePair<string, object>("type", frameworkType);
            var result = GenContext.ToolBox.Repo.GetFrameworks(GenContext.CurrentPlatform)
                .Where(m => m.Tags != null && m.Tags.Contains(type)).ToList();

            return Json(Ok(new { items = result }));
        }
    }
}
