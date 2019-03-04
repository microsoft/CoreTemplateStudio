// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using CoreTemplateStudio.Api.Extensions.Filters;
using CoreTemplateStudio.Api.Models.Generation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core.Gen;

using Newtonsoft.Json.Linq;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateGenContextFilter]
    public class GenerateController : Controller
    {
        [HttpPost]
        public async Task<ActionResult<ContextProvider>> Generate(GenerationData generationData, string projectName, string genPath)
        {
            // TODO: Add projectName and genPath to Generation data, add validation and remove this ifs
            if (string.IsNullOrEmpty(projectName))
            {
                return BadRequest(new { message = "Invalid project name" });
            }

            if (string.IsNullOrEmpty(genPath))
            {
                return BadRequest(new { message = "Invalid generation path" });
            }

            var userSelection = generationData.ToUserSelection();

            ContextProvider provider = new ContextProvider()
            {
                ProjectName = projectName,
                GenerationOutputPath = genPath,
                DestinationPath = genPath,
            };

            GenContext.Current = provider;

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);
            return provider;
        }
    }
}
