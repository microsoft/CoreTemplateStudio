// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : Controller
    {
        [HttpPost]
        public JsonResult Generate([FromBody]UserSelection userSelection)
        {
            return Json(Ok(new { wasUpdated = "dummy data" }));
        }
    }
}
