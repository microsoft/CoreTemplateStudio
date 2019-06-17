using System;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli
{
    public class App
    {
        private readonly Parser _parser;
        private readonly string promptSymbol = ">> ";
        private readonly IMessageService _messageService;
        private readonly IGenerateService _generateService;
        private readonly IGetProjectTypesService _getProjectTypesService;
        private readonly ISyncService _syncService;

        public App(IMessageService messageService, IGenerateService generateService, IGetProjectTypesService getProjectTypesService, ISyncService syncService)
        {
            _messageService = messageService;
            _generateService = generateService;
            _getProjectTypesService = getProjectTypesService;
            _syncService = syncService;

            _parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
        }

        public void Run()
        {            
            bool isRunning = true;

            while (isRunning)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write($"{promptSymbol} ");
                    isRunning = ProcessCommand(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    _messageService.SendError(string.Format(StringRes.ErrorExecutingCommand, ex.Message));
                }
            }
        }

        private bool ProcessCommand(string command)
        {
            var args = command.Split();

            var parserResult = _parser.ParseArguments<SyncOptions, GetProjectTypesOptions, GenerateOptions, CloseOptions>(args);

              var exitCode = parserResult.MapResult(
                    (SyncOptions opts) => _syncService.ProcessAsync(opts),
                    (GetProjectTypesOptions opts) => _getProjectTypesService.ProcessAsync(opts),
                    (GenerateOptions opts) => _generateService.ProcessAsync(opts),
                    (CloseOptions opts) => Task.FromResult(1),
                    errors => {
                        var helpText = HelpText.AutoBuild(parserResult);
                        helpText.AddEnumValuesToHelpText = true;
                        helpText.AddOptions(parserResult);
                        _messageService.SendError(helpText);
                        return Task.FromResult(0);
                    });

            //todo: use async task
            return exitCode.Result == 0;
        }
    }
}
