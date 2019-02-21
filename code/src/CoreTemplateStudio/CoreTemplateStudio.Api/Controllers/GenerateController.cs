// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core.Gen;

using Newtonsoft.Json.Linq;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> Generate([FromBody]JObject userSelection, string projectName, string genPath)
        {
            if (GenContext.ToolBox == null)
            {
                return Json(BadRequest(new { message = "You must first sync templates before calling this endpoint" }));
            }

            if (userSelection == null)
            {
                return Json(BadRequest(new { message = "Invalid user selection" }));
            }
            else if (string.IsNullOrEmpty(projectName))
            {
                return Json(BadRequest(new { message = "Invalid project name" }));
            }
            else if (string.IsNullOrEmpty(genPath))
            {
                return Json(BadRequest(new { message = "Invalid generation path" }));
            }

            UserSelection selection = UserSelectionConverter.FromJObject(userSelection);

            ContextProvider provider = new ContextProvider()
            {
                ProjectName = projectName,
                GenerationOutputPath = genPath,
                DestinationPath = genPath,
            };

            GenContext.Current = provider;

            await NewProjectGenController.Instance.UpdatedUnsafeGenerateProjectAsync(selection);
            return Json(Ok(new { wasUpdated = provider }));
        }
    }
}
