using System;
using Microsoft.Templates.Cli.Services;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<IGenerateService, GenerateService>();
            services.AddTransient<IGetTemplatesService, GetTemplatesService>();
            services.AddTransient<ISyncService, SyncService>();

            // App entry point
            services.AddTransient<App>();

            return services;
        }
    }
}
