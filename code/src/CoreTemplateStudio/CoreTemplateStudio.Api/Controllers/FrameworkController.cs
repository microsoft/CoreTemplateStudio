// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworkController : Controller
    {
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

            var projectFrameworks = GenComposer.GetSupportedFx(projectType, GenContext.CurrentPlatform).ToList();

            var targetFrontEndFrameworks = GenContext.ToolBox
                                                     .Repo
                                                     .GetFrontEndFrameworks()
                                                     .Where(tf => projectFrameworks.Where(framework => framework.Type == FrameworkTypes.FrontEnd)
                                                                  .Any(framework => (string.Equals(framework.Name, tf.Name, StringComparison.OrdinalIgnoreCase)
                                                                                      || "all".Equals(framework.Name, StringComparison.OrdinalIgnoreCase))))
                                                     .ToList();

            var targetBackEndFrameworks = GenContext.ToolBox
                                                     .Repo
                                                     .GetBackEndFrameworks()
                                                     .Where(tf => projectFrameworks.Where(framework => framework.Type == FrameworkTypes.BackEnd)
                                                                  .Any(framework => (string.Equals(framework.Name, tf.Name, StringComparison.OrdinalIgnoreCase)
                                                                                      || "all".Equals(framework.Name, StringComparison.OrdinalIgnoreCase))))
                                                     .ToList();

            List<MetadataInfo> targetFrameworks = new List<MetadataInfo>();

            targetFrameworks.AddRange(targetFrontEndFrameworks);

            targetFrameworks.AddRange(targetBackEndFrameworks);

            return Json(Ok(new { items = targetFrameworks }));
        }
    }
}
