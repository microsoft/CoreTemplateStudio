using System;
using Microsoft.Templates.Cli.Services;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Commands.Dispatcher;
using Microsoft.Templates.Cli.Commands.Handlers;
using System.IO;
using Microsoft.Templates.Cli.Commands.Validators;
using FluentValidation;

namespace Microsoft.Templates.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
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
            services.AddSingleton<ICommandHandler<GetPagesCommand>, GetPagesHandler>();
            services.AddSingleton<ICommandHandler<GetFeaturesCommand>, GetFeaturesHandler>();
            services.AddSingleton<ICommandHandler<GenerateCommand>, GenerateHandler>();

            // Validators
            services.AddSingleton<IValidator<SyncCommand>, SyncValidator>();
            services.AddSingleton<IValidator<GetProjectTypesCommand>, GetProjectTypesValidator>();
            services.AddSingleton<IValidator<GetFrameworksCommand>, GetFrameworksValidator>();
            services.AddSingleton<IValidator<GetPagesCommand>, GetPagesValidator>();
            services.AddSingleton<IValidator<GetFeaturesCommand>, GetFeaturesValidator>();
            services.AddSingleton<IValidator<GenerateCommand>, GenerateValidator>();

            // App entry point
            services.AddTransient<App>();

            return services;
        }
    }
}
