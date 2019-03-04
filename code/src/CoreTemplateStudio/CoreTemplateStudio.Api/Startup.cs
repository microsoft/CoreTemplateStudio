// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CoreTemplateStudio.Api.Extensions.Filters;
using CoreTemplateStudio.Api.Extensions.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<ValidateGenContextFilter>();

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

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Disable automatic model validation to use ValidateModelStateFilter
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
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
              .UseMvc();
        }
    }
}
