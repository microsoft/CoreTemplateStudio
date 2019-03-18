// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using CoreTemplateStudio.Api.Extensions.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates;
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
        [HttpGet]
        public ActionResult<List<MetadataInfo>> GetFrameworks(string projectType)
        {
            if (projectType == null)
            {
                return BadRequest(new { message = StringRes.BadReqInvalidProjectType });
            }

            var projectFrameworks = GenComposer.GetSupportedFx(projectType, GenContext.CurrentPlatform).ToList();

            var targetFrontEndFrameworks = GenContext.ToolBox
                                                     .Repo
                                                     .GetFrontEndFrameworks()
                                                     .Where(tf => projectFrameworks.Where(framework => framework.Type == FrameworkTypes.FrontEnd)
                                                                  .Any(framework => (string.Equals(framework.Name, tf.Name, StringComparison.OrdinalIgnoreCase)
                                                                                      || "all".Equals(framework.Name, StringComparison.OrdinalIgnoreCase))));

            var targetBackEndFrameworks = GenContext.ToolBox
                                                     .Repo
                                                     .GetBackEndFrameworks()
                                                     .Where(tf => projectFrameworks.Where(framework => framework.Type == FrameworkTypes.BackEnd)
                                                                  .Any(framework => (string.Equals(framework.Name, tf.Name, StringComparison.OrdinalIgnoreCase)
                                                                                      || "all".Equals(framework.Name, StringComparison.OrdinalIgnoreCase))));

            List<MetadataInfo> targetFrameworks = new List<MetadataInfo>();
            targetFrameworks.AddRange(targetFrontEndFrameworks);
            targetFrameworks.AddRange(targetBackEndFrameworks);

            return targetFrameworks;
        }
    }
}
