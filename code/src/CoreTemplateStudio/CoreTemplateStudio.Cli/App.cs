using System;
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
                    (SyncOptions opts) => _syncService.Process(opts),
                    (GetTemplatesOptions opts) => _getTemplatesService.Process(opts),
                    (GenerateOptions opts) => _generateService.Process(opts),
                    (CloseOptions opts) => 1,
                    errors => 0);

            return result == 0;
        }
    }
}
