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
        private readonly IDictionary<ShortFramework, FrameworkItem> frameworkStore;

        public FrameworkController()
        {
            frameworkStore = new Dictionary<ShortFramework, FrameworkItem>
            {
                { ShortFramework.RJS, new FrameworkItem(ShortFramework.RJS, FrameworkType.Frontend, ShortProjectType.SPFE, ShortProjectType.MPFE, ShortProjectType.MPAFS, ShortProjectType.SPAFS) },
                { ShortFramework.VJS, new FrameworkItem(ShortFramework.VJS, FrameworkType.Frontend, ShortProjectType.SPFE, ShortProjectType.MPFE, ShortProjectType.MPAFS, ShortProjectType.SPAFS) },
                { ShortFramework.AJS, new FrameworkItem(ShortFramework.AJS, FrameworkType.Frontend, ShortProjectType.SPFE, ShortProjectType.MPFE, ShortProjectType.MPAFS, ShortProjectType.SPAFS) },
                { ShortFramework.NJS, new FrameworkItem(ShortFramework.NJS, FrameworkType.Backend, ShortProjectType.SPFE, ShortProjectType.MPFE, ShortProjectType.MPAFS, ShortProjectType.SPAFS, ShortProjectType.REST) },
                { ShortFramework.DJG, new FrameworkItem(ShortFramework.DJG, FrameworkType.Backend, ShortProjectType.SPFE, ShortProjectType.MPFE, ShortProjectType.MPAFS, ShortProjectType.SPAFS, ShortProjectType.REST) },
                { ShortFramework.MPJS, new FrameworkItem(ShortFramework.MPJS, FrameworkType.Frontend, ShortProjectType.MPFE, ShortProjectType.MPAFS) },
                { ShortFramework.SPJS, new FrameworkItem(ShortFramework.SPJS, FrameworkType.Frontend, ShortProjectType.SPFE, ShortProjectType.SPAFS) },
            };
        }

        // GET: api/framework?projectType=
        // returns all frameworks matching a given projectType Code
        [HttpGet]
        public JsonResult GetFrameworks(string projectType)
        {
            IDictionary<ShortFramework, FrameworkItem> validFrameworks = new Dictionary<ShortFramework, FrameworkItem>();
            foreach (var item in frameworkStore)
            {
                Enum.TryParse(projectType, out ShortProjectType shortProjectType);
                if (item.Value.ProjectTypes.Contains(shortProjectType))
                {
                    validFrameworks.Add(item.Key, item.Value);
                }
            }

            return Json(validFrameworks);
        }

        // GET: api/framework/frontend?projectType=
        // returns all frontend frameworks matching a given projectType Code
        [HttpGet("frontend")]
        public JsonResult GetFrontendFrameworks(string projectType)
        {
            IDictionary<ShortFramework, FrameworkItem> validFrameworks = new Dictionary<ShortFramework, FrameworkItem>();
            foreach (var item in frameworkStore)
            {
                Enum.TryParse(projectType, out ShortProjectType shortProjectType);
                if (item.Value.FrameworkType.ToLower() == "frontend" && item.Value.ProjectTypes.Contains(shortProjectType))
                {
                    validFrameworks.Add(item.Key, item.Value);
                }
            }

            return Json(validFrameworks);
        }

        // GET: api/framework/backend?projectType=
        // returns all backend frameworks matching a given projectType Code
        [HttpGet("backend")]
        public JsonResult GetBackendFrameworks(string projectType)
        {
            IDictionary<ShortFramework, FrameworkItem> validFrameworks = new Dictionary<ShortFramework, FrameworkItem>();
            foreach (var item in frameworkStore)
            {
                Enum.TryParse(projectType, out ShortProjectType shortProjectType);
                if (item.Value.FrameworkType.ToLower() == "backend" && item.Value.ProjectTypes.Contains(shortProjectType))
                {
                    validFrameworks.Add(item.Key, item.Value);
                }
            }

            return Json(validFrameworks);
        }
    }
}
