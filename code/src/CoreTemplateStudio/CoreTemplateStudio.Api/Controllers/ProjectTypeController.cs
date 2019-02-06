// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;
using CoreTemplateStudio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : Controller
    {
        private readonly IDictionary<ProjectType, ProjectTypeModel> projectTypeStore;

        public ProjectTypeController()
        {
            projectTypeStore = new Dictionary<ProjectType, ProjectTypeModel>
            {
                { ProjectType.SinglePageFull, new ProjectTypeModel(ProjectType.SinglePageFull, Platform.Web) },
                { ProjectType.MultiPageFull, new ProjectTypeModel(ProjectType.MultiPageFull, Platform.Web) },
                { ProjectType.SinglePageFront, new ProjectTypeModel(ProjectType.SinglePageFront, Platform.Web) },
                { ProjectType.MultiPageFront, new ProjectTypeModel(ProjectType.MultiPageFront, Platform.Web) },
                { ProjectType.RESTAPI, new ProjectTypeModel(ProjectType.RESTAPI, Platform.Web) },
                { ProjectType.NavPaneCSharp, new ProjectTypeModel(ProjectType.NavPaneCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.NavPaneVB, new ProjectTypeModel(ProjectType.NavPaneVB, Platform.Uwp, Language.VB) },
                { ProjectType.BlankCSharp, new ProjectTypeModel(ProjectType.BlankCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.BlankVB, new ProjectTypeModel(ProjectType.BlankVB, Platform.Uwp, Language.VB) },
                { ProjectType.PivotTabCSharp, new ProjectTypeModel(ProjectType.PivotTabCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.PivotTabVB, new ProjectTypeModel(ProjectType.PivotTabVB, Platform.Uwp, Language.VB) },
            };
        }

        // GET: api/projectType?platform=<>&language=<>
        // returns all project types matching the given platform and language as JSON
        // platform if required. Specifying language is required if platformType is Uwp, will be ignored for web
        [HttpGet]
        public JsonResult GetProjectTypes(string platform = "", string language = "")
        {
            // these are temporary, will be using data from the engine (language and platform classes)
            // directly and not random string comparisons.
            if (!platform.Equals("uwp", StringComparison.OrdinalIgnoreCase) && !platform.Equals("web", StringComparison.OrdinalIgnoreCase))
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }

            if (platform.Equals("uwp", StringComparison.OrdinalIgnoreCase))
            {
                if (!language.Equals("csharp", StringComparison.OrdinalIgnoreCase) && !language.Equals("vb", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(BadRequest(new { message = "invalid language for platform: uwp" }));
                }
            }

            Enum.TryParse(platform, true, out Platform pform);
            Enum.TryParse(language, true, out Language lang);

            IDictionary<ProjectType, ProjectTypeModel> validProjects = new Dictionary<ProjectType, ProjectTypeModel>();
            foreach (var item in projectTypeStore)
            {
                if (item.Value.IsPlatform(pform) && item.Value.IsLanguage(lang))
                {
                    validProjects.Add(item);
                }
            }

            return Json(Ok(validProjects));
        }
    }
}
