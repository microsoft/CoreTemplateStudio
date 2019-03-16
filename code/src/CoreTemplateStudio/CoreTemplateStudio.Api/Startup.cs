// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CoreTemplateStudio.Api.Extensions.Filters;
using CoreTemplateStudio.Api.Extensions.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Api.Hubs;

namespace CoreTemplateStudio.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ValidateGenContextFilter>();

            // Adds Cors policy for swagger docs response, since this is a local server this should be fine.
            services.AddCors(options =>
             {
                 options.AddPolicy(
                     "AllowAll",
                     builder =>
                     {
                         builder
                         .AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials();
                     });
             });

            services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Validation controller models
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context => GetValidationModelError(context);
            });

            // SignalR for live data.
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
              .UseMiddleware<ErrorHandlerMiddleware>()
              .UseCors("AllowAll")
              .UseMvc()
              .UseSignalR(route =>
              {
                  route.MapHub<CoreHub>("/corehub");
              });
        }

        public static BadRequestObjectResult GetValidationModelError(ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Type = $"https://httpstatuses.com/400",
                Detail = "Invalid input data. See additional details in 'errors' property.",
            };

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json", "application/problem+xml" },
            };
        }
    }
}
