// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        private readonly IDictionary<ShortProjectType, ProjectTypeItem> projectTypeStore;

        public ProjectTypeController()
        {
            projectTypeStore = new Dictionary<ShortProjectType, ProjectTypeItem>
            {
                { ShortProjectType.SPAFS, new ProjectTypeItem(ShortProjectType.SPAFS) },
                { ShortProjectType.MPAFS, new ProjectTypeItem(ShortProjectType.MPAFS) },
                { ShortProjectType.SPFE, new ProjectTypeItem(ShortProjectType.SPFE) },
                { ShortProjectType.MPFE, new ProjectTypeItem(ShortProjectType.MPFE) },
                { ShortProjectType.REST, new ProjectTypeItem(ShortProjectType.REST) },
            };
        }

        // GET: api/projectType
        // returns all project types matching the given platform and language as JSON
        [HttpGet]
        public JsonResult GetProjectTypes(string platform, string language)
        {
            return Json(projectTypeStore);
        }
    }
}
