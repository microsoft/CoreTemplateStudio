// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using CoreTemplateStudio.Api.Extensions.Filters;
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
        public async Task<ActionResult<ContextProvider>> Generate([FromBody] JObject userSelection, string projectName, string genPath)
        {
            if (userSelection == null)
            {
                return BadRequest(new { message = "Invalid user selection" });
            }

            if (string.IsNullOrEmpty(projectName))
            {
                return BadRequest(new { message = "Invalid project name" });
            }

            if (string.IsNullOrEmpty(genPath))
            {
                return BadRequest(new { message = "Invalid generation path" });
            }

            // TODO: User selection shold be obtained from params and delete UserSelectionConverter
            UserSelection selection = UserSelectionConverter.FromJObject(userSelection);

            ContextProvider provider = new ContextProvider()
            {
                ProjectName = projectName,
                GenerationOutputPath = genPath,
                DestinationPath = genPath,
            };

            GenContext.Current = provider;

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(selection);
            return provider;
        }
    }
}
