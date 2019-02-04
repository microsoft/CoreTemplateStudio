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
        private readonly IDictionary<ProjectType, ProjectTypeItem> projectTypeStore;

        public ProjectTypeController()
        {
            projectTypeStore = new Dictionary<ProjectType, ProjectTypeItem>
            {
                { ProjectType.SinglePageFull, new ProjectTypeItem(ProjectType.SinglePageFull, Platform.Web) },
                { ProjectType.MultiPageFull, new ProjectTypeItem(ProjectType.MultiPageFull, Platform.Web) },
                { ProjectType.SinglePageFront, new ProjectTypeItem(ProjectType.SinglePageFront, Platform.Web) },
                { ProjectType.MultiPageFront, new ProjectTypeItem(ProjectType.MultiPageFront, Platform.Web) },
                { ProjectType.RESTAPI, new ProjectTypeItem(ProjectType.RESTAPI, Platform.Web) },
                { ProjectType.NavPaneCSharp, new ProjectTypeItem(ProjectType.NavPaneCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.NavPaneVB, new ProjectTypeItem(ProjectType.NavPaneVB, Platform.Uwp, Language.VB) },
                { ProjectType.BlankCSharp, new ProjectTypeItem(ProjectType.BlankCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.BlankVB, new ProjectTypeItem(ProjectType.BlankVB, Platform.Uwp, Language.VB) },
                { ProjectType.PivotTabCSharp, new ProjectTypeItem(ProjectType.PivotTabCSharp, Platform.Uwp, Language.CSharp) },
                { ProjectType.PivotTabVB, new ProjectTypeItem(ProjectType.PivotTabVB, Platform.Uwp, Language.VB) },
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

            IDictionary<ProjectType, ProjectTypeItem> validProjects = new Dictionary<ProjectType, ProjectTypeItem>();
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
