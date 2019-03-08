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
                context.Result = GetError(context);
                return;
            }

            await next();
        }

        private ObjectResult GetError(ActionExecutingContext context)
        {
            var statusCode = StatusCodes.Status403Forbidden;

            var problemDetails = new ProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
                Detail = "You must first sync templates before calling this endpoint.",
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode,
                ContentTypes =
                {
                    "application/problem+json",
                    "application/problem+xml",
                },
            };
        }
    }
}
