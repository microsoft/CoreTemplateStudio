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
    public class FrameworkController : Controller
    {
        private readonly IDictionary<Framework, FrameworkItem> frameworkStore;

        public FrameworkController()
        {
            frameworkStore = new Dictionary<Framework, FrameworkItem>
            {
                { Framework.RJS, new FrameworkItem(Framework.RJS, FrameworkType.Frontend, ProjectType.SPFE, ProjectType.MPFE, ProjectType.MPAFS, ProjectType.SPAFS) },
                { Framework.VJS, new FrameworkItem(Framework.VJS, FrameworkType.Frontend, ProjectType.SPFE, ProjectType.MPFE, ProjectType.MPAFS, ProjectType.SPAFS) },
                { Framework.AJS, new FrameworkItem(Framework.AJS, FrameworkType.Frontend, ProjectType.SPFE, ProjectType.MPFE, ProjectType.MPAFS, ProjectType.SPAFS) },
                { Framework.NJS, new FrameworkItem(Framework.NJS, FrameworkType.Backend, ProjectType.SPFE, ProjectType.MPFE, ProjectType.MPAFS, ProjectType.SPAFS, ProjectType.REST) },
                { Framework.DJG, new FrameworkItem(Framework.DJG, FrameworkType.Backend, ProjectType.SPFE, ProjectType.MPFE, ProjectType.MPAFS, ProjectType.SPAFS, ProjectType.REST) },
                { Framework.MPJS, new FrameworkItem(Framework.MPJS, FrameworkType.Frontend, ProjectType.MPFE, ProjectType.MPAFS) },
                { Framework.SPJS, new FrameworkItem(Framework.SPJS, FrameworkType.Frontend, ProjectType.SPFE, ProjectType.SPAFS) },
                { Framework.CBH, new FrameworkItem(Framework.CBH, FrameworkType.UwpDesign, ProjectType.BVB, ProjectType.BCS, ProjectType.PTCS, ProjectType.PTVB, ProjectType.NAVCS, ProjectType.NAVVB) },
                { Framework.MVVMB, new FrameworkItem(Framework.MVVMB, FrameworkType.UwpDesign, ProjectType.BVB, ProjectType.BCS, ProjectType.PTCS, ProjectType.PTVB, ProjectType.NAVCS, ProjectType.NAVVB) },
                { Framework.MVVML, new FrameworkItem(Framework.MVVML, FrameworkType.UwpDesign, ProjectType.BVB, ProjectType.BCS, ProjectType.PTCS, ProjectType.PTVB, ProjectType.NAVCS, ProjectType.NAVVB) },
                { Framework.PRSM, new FrameworkItem(Framework.PRSM, FrameworkType.UwpDesign, ProjectType.BVB, ProjectType.BCS, ProjectType.PTCS, ProjectType.PTVB, ProjectType.NAVCS, ProjectType.NAVVB) },
                { Framework.CBM, new FrameworkItem(Framework.CBM, FrameworkType.UwpDesign, ProjectType.BVB, ProjectType.BCS, ProjectType.PTCS, ProjectType.PTVB, ProjectType.NAVCS, ProjectType.NAVVB) },
            };
        }

        // GET: api/framework?projectType=
        // returns all frameworks matching a given projectType Code
        [HttpGet]
        public JsonResult GetFrameworks(string projectType)
        {
            IDictionary<Framework, FrameworkItem> validFrameworks = new Dictionary<Framework, FrameworkItem>();
            foreach (var item in frameworkStore)
            {
                Enum.TryParse(projectType, true, out ProjectType parsedProjectType);
                if (item.Value.HasProjectType(parsedProjectType))
                {
                    validFrameworks.Add(item);
                }
            }

            return Json(Ok(validFrameworks));
        }

        // GET: api/framework/{frameworkType}?projectType=
        // {frameworkType} is either frontend/backend/uwpDesign
        // returns all frontend/backend frameworks matching a given projectType Code
        [HttpGet("{frameworkType}")]
        public JsonResult GetFrameworksOfType(string frameworkType, string projectType)
        {
            if (frameworkType.ToLower() != "frontend" && frameworkType.ToLower() != "backend" && frameworkType.ToLower() != "uwpdesign")
            {
                return Json(BadRequest(new { message = "invalid framework type" }));
            }

            IDictionary<Framework, FrameworkItem> validFrameworks = new Dictionary<Framework, FrameworkItem>();
            foreach (var item in frameworkStore)
            {
                Enum.TryParse(projectType, true, out ProjectType parsedProjectType);
                if (item.Value.FrameworkType.ToLower() == frameworkType.ToLower() && item.Value.HasProjectType(parsedProjectType))
                {
                    validFrameworks.Add(item);
                }
            }

            return Json(Ok(validFrameworks));
        }
    }
}
