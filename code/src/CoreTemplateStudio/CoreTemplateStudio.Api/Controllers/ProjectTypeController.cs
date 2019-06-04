// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using CoreTemplateStudio.Api.Extensions.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateGenContextFilter]
    public class ProjectTypeController : Controller
    {
        [EnableCors("AllowAll")]
        [HttpGet]
        public List<MetadataInfo> GetProjectTypes()
        {
            var platform = GenContext.CurrentPlatform;
            var result = GenContext.ToolBox.Repo.GetProjectTypes(platform);

            return result.ToList();
        }
    }
}
