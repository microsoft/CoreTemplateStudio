using System;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli
{
    public class App
    {
        private readonly string promptSymbol = ">> ";
        private readonly ICommandDispatcher _dispatcher;
        private readonly IMessageService _messageService;
        private readonly IGenerateService _generateService;
        private readonly ISyncService _syncService;

        public App(ICommandDispatcher dispatcher, IMessageService messageService, IGenerateService generateService, ISyncService syncService)
        {
            _dispatcher = dispatcher;
            _messageService = messageService;
            _generateService = generateService;
            _syncService = syncService;
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

            var parserResult = Parser.Default.ParseArguments<SyncCommand, GetProjectTypesCommand, GetFrameworksCommand, GetPagesCommand, GetFeaturesCommand, GenerateOptions, CloseOptions>(args);

              var exitCode = parserResult.MapResult(
                    (SyncCommand opts) => _dispatcher.DispatchAsync(opts),
                    (GetProjectTypesCommand opts) => _dispatcher.DispatchAsync(opts),
                    (GetFrameworksCommand opts) => _dispatcher.DispatchAsync(opts),
                    (GetPagesCommand opts) => _dispatcher.DispatchAsync(opts),
                    (GetFeaturesCommand opts) => _dispatcher.DispatchAsync(opts),
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
