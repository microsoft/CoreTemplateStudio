// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworkController : Controller
    {
        /// <summary>
        /// GET: api/framework?projectType=<>
        /// Gets all frameworks available for the current platform and language for the given projectType.
        /// </summary>
        /// <param name="projectType">Project Type that was selected by the user</param>
        /// <returns>all frameworks for the current platform and language for that project type</returns>
        [HttpGet]
        public JsonResult GetFrameworks(string projectType)
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            if (projectType == null)
            {
                return Json(BadRequest(new { message = "Invalid project type" }));
            }

            var projectFrameworks = GenComposer.GetAllSupportedFx(projectType, GenContext.CurrentPlatform).ToList();

            var targetFrontEndFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks()
                                                                .Where(tf => projectFrameworks.Contains(tf.Name)).ToList();

            var targetBackEndFrameworks = GenContext.ToolBox.Repo.GetBackEndFrameworks()
                                                                .Where(tf => projectFrameworks.Contains(tf.Name)).ToList();

            List<MetadataInfo> targetFrameworks = new List<MetadataInfo>();

            targetFrameworks.AddRange(targetFrontEndFrameworks);

            targetFrameworks.AddRange(targetBackEndFrameworks);

            return Json(Ok(new { items = targetFrameworks }));
        }
    }
}
