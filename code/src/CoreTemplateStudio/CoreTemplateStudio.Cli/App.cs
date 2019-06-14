using System;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli
{
    public class App
    {
        private readonly string promptSymbol = ">> ";
        private readonly string commandSeparator = " ";
        private readonly IGenerateService _generateService;
        private readonly IGetTemplatesService _getTemplatesService;
        private readonly ISyncService _syncService;

        public App(IGenerateService generateService, IGetTemplatesService getTemplatesService, ISyncService syncService)
        {
            _generateService = generateService;
            _getTemplatesService = getTemplatesService;
            _syncService = syncService;
        }

        public void Run()
        {            
            bool isRunning = true;

            while(isRunning)
            {
                Console.WriteLine();
                Console.Write($"{promptSymbol} ");
                isRunning = ProcessCommand(Console.ReadLine());
            }
        }

        private bool ProcessCommand(string command)
        {
            var args = command.Split(commandSeparator);

            var result = Parser.Default
                .ParseArguments<SyncOptions, GetTemplatesOptions, GenerateOptions, CloseOptions>(args)
                .MapResult(
                    async (SyncOptions opts) => await _syncService.ProcessAsync(opts),
                    async (GetTemplatesOptions opts) => await _getTemplatesService.ProcessAsync(opts),
                    async (GenerateOptions opts) => await _generateService.ProcessAsync(opts),
                    (CloseOptions opts) => Task.FromResult(1),
                    errors => Task.FromResult(0));

            //todo: use async task
            return result.Result == 0;
        }
    }
}
