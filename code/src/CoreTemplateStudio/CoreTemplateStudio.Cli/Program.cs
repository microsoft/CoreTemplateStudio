using System;
using Microsoft.Templates.Cli.Services;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Commands.Dispatcher;
using Microsoft.Templates.Cli.Commands.Handlers;

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

            // App entry point
            services.AddTransient<App>();

            return services;
        }
    }
}
