// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public static Task WriteModelAsync<TModel>(this HttpContext context, TModel model)
        {
            var result = new ObjectResult(model)
            {
                DeclaredType = typeof(TModel),
            };

            return context.ExecuteResultAsync(result);
        }

        public static Task ExecuteResultAsync<TResult>(this HttpContext context, TResult result)
            where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<TResult>>();

            var routeData = context.GetRouteData() ?? EmptyRouteData;
            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }
}
