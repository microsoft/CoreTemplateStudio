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
        private readonly IDictionary<Framework, FrameworkModel> frameworkStore;

        public FrameworkController()
        {
            frameworkStore = new Dictionary<Framework, FrameworkModel>
            {
                { Framework.ReactJS, new FrameworkModel(Framework.ReactJS, FrameworkType.Frontend, ProjectType.SinglePageFront, ProjectType.MultiPageFront, ProjectType.MultiPageFull, ProjectType.SinglePageFull) },
                { Framework.VueJS, new FrameworkModel(Framework.VueJS, FrameworkType.Frontend, ProjectType.SinglePageFront, ProjectType.MultiPageFront, ProjectType.MultiPageFull, ProjectType.SinglePageFull) },
                { Framework.AngularJS, new FrameworkModel(Framework.AngularJS, FrameworkType.Frontend, ProjectType.SinglePageFront, ProjectType.MultiPageFront, ProjectType.MultiPageFull, ProjectType.SinglePageFull) },
                { Framework.NodeJS, new FrameworkModel(Framework.NodeJS, FrameworkType.Backend, ProjectType.SinglePageFront, ProjectType.MultiPageFront, ProjectType.MultiPageFull, ProjectType.SinglePageFull, ProjectType.RESTAPI) },
                { Framework.Django, new FrameworkModel(Framework.Django, FrameworkType.Backend, ProjectType.SinglePageFront, ProjectType.MultiPageFront, ProjectType.MultiPageFull, ProjectType.SinglePageFull, ProjectType.RESTAPI) },
                { Framework.MultiPageJS, new FrameworkModel(Framework.MultiPageJS, FrameworkType.Frontend, ProjectType.MultiPageFront, ProjectType.MultiPageFull) },
                { Framework.SinglePageJS, new FrameworkModel(Framework.SinglePageJS, FrameworkType.Frontend, ProjectType.SinglePageFront, ProjectType.SinglePageFull) },
                { Framework.CodeBehind, new FrameworkModel(Framework.CodeBehind, FrameworkType.UwpDesign, ProjectType.BlankVB, ProjectType.BlankCSharp, ProjectType.PivotTabCSharp, ProjectType.PivotTabVB, ProjectType.NavPaneCSharp, ProjectType.NavPaneVB) },
                { Framework.MVVMBasic, new FrameworkModel(Framework.MVVMBasic, FrameworkType.UwpDesign, ProjectType.BlankVB, ProjectType.BlankCSharp, ProjectType.PivotTabCSharp, ProjectType.PivotTabVB, ProjectType.NavPaneCSharp, ProjectType.NavPaneVB) },
                { Framework.MVVMLight, new FrameworkModel(Framework.MVVMLight, FrameworkType.UwpDesign, ProjectType.BlankVB, ProjectType.BlankCSharp, ProjectType.PivotTabCSharp, ProjectType.PivotTabVB, ProjectType.NavPaneCSharp, ProjectType.NavPaneVB) },
                { Framework.Prism, new FrameworkModel(Framework.Prism, FrameworkType.UwpDesign, ProjectType.BlankVB, ProjectType.BlankCSharp, ProjectType.PivotTabCSharp, ProjectType.PivotTabVB, ProjectType.NavPaneCSharp, ProjectType.NavPaneVB) },
                { Framework.CaliburnMicro, new FrameworkModel(Framework.CaliburnMicro, FrameworkType.UwpDesign, ProjectType.BlankVB, ProjectType.BlankCSharp, ProjectType.PivotTabCSharp, ProjectType.PivotTabVB, ProjectType.NavPaneCSharp, ProjectType.NavPaneVB) },
            };
        }

        // GET: api/framework?projectType=
        // returns all frameworks matching a given projectType Code
        [HttpGet]
        public JsonResult GetFrameworks(string projectType)
        {
            if (projectType == null)
            {
                return Json(BadRequest(new { message = "please specify a valid projectType" }));
            }

            IDictionary<Framework, FrameworkModel> validFrameworks = new Dictionary<Framework, FrameworkModel>();
            foreach (var item in frameworkStore)
            {
                if (Enum.TryParse(projectType, true, out ProjectType parsedProjectType))
                {
                    if (item.Value.HasProjectType(parsedProjectType))
                    {
                        validFrameworks.Add(item);
                    }
                }
                else
                {
                    return Json(BadRequest(new { message = "please specify a valid projectType" }));
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
            if (projectType == null)
            {
                return Json(BadRequest(new { message = "please specify a valid projectType" }));
            }

            if (!frameworkType.Equals("frontend", StringComparison.OrdinalIgnoreCase) && !frameworkType.Equals("backend", StringComparison.OrdinalIgnoreCase) && !frameworkType.Equals("uwpdesign", StringComparison.OrdinalIgnoreCase))
            {
                return Json(BadRequest(new { message = "invalid framework type" }));
            }

            IDictionary<Framework, FrameworkModel> validFrameworks = new Dictionary<Framework, FrameworkModel>();
            foreach (var item in frameworkStore)
            {
                if (Enum.TryParse(projectType, true, out ProjectType parsedProjectType))
                {
                    if (item.Value.FrameworkType.Equals(frameworkType, StringComparison.OrdinalIgnoreCase) && item.Value.HasProjectType(parsedProjectType))
                    {
                        validFrameworks.Add(item);
                    }
                }
                else
                {
                    return Json(BadRequest(new { message = "please specify a valid projectType" }));
                }
            }

            return Json(Ok(validFrameworks));
        }
    }
}
