// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Extensions.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problemDetailResult = GetError(context, exception);
            await context.ExecuteResultAsync(problemDetailResult);
        }

        private static ObjectResult GetError(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Status = context.Response.StatusCode,
                Type = $"https://httpstatuses.com/{context.Response.StatusCode}",
                Detail = $"Internal Server Error from the custom middleware. {exception.Message}",
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = context.Response.StatusCode,
                ContentTypes =
                {
                    "application/problem+json",
                    "application/problem+xml",
                },
            };
        }
    }
}
