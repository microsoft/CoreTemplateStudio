// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : Controller
    {
        /// <summary>
        /// GET: api/projectType
        /// Gets available project types for the current platform and language.
        /// </summary>
        /// <returns>all the project types for the current platform and language. The parameters defined in sync will be used.</returns>
        [HttpGet]
        public JsonResult GetProjectTypes()
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            return Json(Ok(new { items = GenContext.ToolBox.Repo.GetProjectTypes().ToList() }));
        }
    }
}
