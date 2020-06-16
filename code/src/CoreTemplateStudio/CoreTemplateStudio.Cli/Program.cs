// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Commands.Dispatcher;
using Microsoft.Templates.Cli.Commands.Handlers;
using Microsoft.Templates.Cli.Commands.Validators;
using Microsoft.Templates.Cli.Services;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            IncreaseConsoleReadLineBuffer();
#endif

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<App>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            // Services
            services.AddSingleton<IGenerateService, GenerateService>();
            services.AddSingleton<ITemplatesService, TemplatesService>();
            services.AddSingleton<ISyncService, SyncService>();
            services.AddSingleton<IMessageService, MessageService>();

            // Commands
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<ICommandHandler<SyncCommand>, SyncHandler>();
            services.AddSingleton<ICommandHandler<GetProjectTypesCommand>, GetProjectTypesHandler>();
            services.AddSingleton<ICommandHandler<GetFrameworksCommand>, GetFrameworksHandler>();
            services.AddSingleton<ICommandHandler<GetLayoutsCommand>, GetLayoutsHandler>();
            services.AddSingleton<ICommandHandler<GetPagesCommand>, GetPagesHandler>();
            services.AddSingleton<ICommandHandler<GetFeaturesCommand>, GetFeaturesHandler>();
            services.AddSingleton<ICommandHandler<GetServicesCommand>, GetServicesHandler>();
            services.AddSingleton<ICommandHandler<GetTestingsCommand>, GetTestingsHandler>();
            services.AddSingleton<ICommandHandler<GenerateCommand>, GenerateHandler>();

            // Validators
            services.AddSingleton<IValidator<SyncCommand>, SyncValidator>();
            services.AddSingleton<IValidator<GetProjectTypesCommand>, GetProjectTypesValidator>();
            services.AddSingleton<IValidator<GetFrameworksCommand>, GetFrameworksValidator>();
            services.AddSingleton<IValidator<GetLayoutsCommand>, GetLayoutsValidator>();
            services.AddSingleton<IValidator<GetPagesCommand>, GetPagesValidator>();
            services.AddSingleton<IValidator<GetFeaturesCommand>, GetFeaturesValidator>();
            services.AddSingleton<IValidator<GetServicesCommand>, GetServicesValidator>();
            services.AddSingleton<IValidator<GetTestingsCommand>, GetTestingsValidator>();
            services.AddSingleton<IValidator<GenerateCommand>, GenerateValidator>();

            // App entry point
            services.AddTransient<App>();

            return services;
        }

        private static void IncreaseConsoleReadLineBuffer()
        {
            byte[] inputBuffer = new byte[4096];
            Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
        }
    }
}
