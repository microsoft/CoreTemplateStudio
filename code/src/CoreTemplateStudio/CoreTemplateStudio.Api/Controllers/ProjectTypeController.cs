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
                { ProjectType.SPAFS, new ProjectTypeItem(ProjectType.SPAFS, Platform.Web) },
                { ProjectType.MPAFS, new ProjectTypeItem(ProjectType.MPAFS, Platform.Web) },
                { ProjectType.SPFE, new ProjectTypeItem(ProjectType.SPFE, Platform.Web) },
                { ProjectType.MPFE, new ProjectTypeItem(ProjectType.MPFE, Platform.Web) },
                { ProjectType.REST, new ProjectTypeItem(ProjectType.REST, Platform.Web) },
                { ProjectType.NAVCS, new ProjectTypeItem(ProjectType.NAVCS, Platform.Uwp, Language.CSharp) },
                { ProjectType.NAVVB, new ProjectTypeItem(ProjectType.NAVVB, Platform.Uwp, Language.VB) },
                { ProjectType.BCS, new ProjectTypeItem(ProjectType.BCS, Platform.Uwp, Language.CSharp) },
                { ProjectType.BVB, new ProjectTypeItem(ProjectType.BVB, Platform.Uwp, Language.VB) },
                { ProjectType.PTCS, new ProjectTypeItem(ProjectType.PTCS, Platform.Uwp, Language.CSharp) },
                { ProjectType.PTVB, new ProjectTypeItem(ProjectType.PTVB, Platform.Uwp, Language.VB) },
            };
        }

        // GET: api/projectType?platform=<>&language=<>
        // returns all project types matching the given platform and language as JSON
        // platform if required. Specifying language is required if platformType is Uwp, will be ignored for web
        [HttpGet]
        public JsonResult GetProjectTypes(string platform = "", string language = "")
        {
            platform = platform.ToLower();
            language = language.ToLower();

            // these are temporary, will be using data from the engine (language and platform classes)
            // directly and not random string comparisons.
            if (platform != "uwp" && platform != "web")
            {
                return Json(BadRequest(new { message = "invalid platform" }));
            }

            if (platform == "uwp")
            {
                if (language != "csharp" && language != "vb")
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
