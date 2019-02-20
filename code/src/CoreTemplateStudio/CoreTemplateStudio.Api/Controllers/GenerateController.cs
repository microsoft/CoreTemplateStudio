// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// <summary>
        /// Post: UserSelection in PostBody /api/generate?projectName=<>&genPath=<>
        /// Given a UserSelection object, project name and genpath generates a project using the given parameters,
        /// at the specified generation path.
        /// </summary>
        /// <param name="userSelection">UserSelection object passed as json.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="genPath">The path the project should go to</param>
        /// <returns>Context provider, or error message.</returns>
        [HttpPost]
        public JsonResult Generate([FromBody]JObject userSelection, string projectName, string genPath)
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

            NewProjectGenController.Instance.UpdatedUnsafeGenerateProjectAsync(selection).Wait();
            return Json(Ok(new { wasUpdated = provider }));
        }
    }
}
