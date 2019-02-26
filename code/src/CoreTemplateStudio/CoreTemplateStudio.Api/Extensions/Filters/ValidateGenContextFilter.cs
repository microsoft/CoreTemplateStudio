// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Extensions.Filters
{
    public class ValidateGenContextFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (GenContext.ToolBox == null)
            {
                var result = new ObjectResult("You must first sync templates before calling this endpoint")
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };

                context.Result = result;
                return;
            }

            await next();
        }
    }
}
