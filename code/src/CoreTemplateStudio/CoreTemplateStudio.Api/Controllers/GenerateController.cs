// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using CoreTemplateStudio.Api.Extensions.Filters;
using CoreTemplateStudio.Api.Models.Generation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateGenContextFilter]
    public class GenerateController : Controller
    {
        [HttpPost]
        public async Task<ActionResult<ContextProvider>> Generate(GenerationData generationData)
        {
            ContextProvider provider = new ContextProvider()
            {
                ProjectName = generationData.ProjectName,
                GenerationOutputPath = generationData.GenPath,
                DestinationPath = generationData.GenPath,
            };

            GenContext.Current = provider;

            var userSelection = generationData.ToUserSelection();
            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            // TODO: We should generationOutputPath??
            return provider;
        }
    }
}
