// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CoreTemplateStudio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : Controller
    {
        [HttpPost]
        public JsonResult Generate([FromBody]GenerateModel generationHelper)
        {
            (bool valid, string error) = generationHelper.Validate();
            if (!valid)
            {
                return Json(BadRequest(new { message = error }));
            }

            return Json(Ok(new { wasUpdated = generationHelper.Generate() }));
        }
    }
}
