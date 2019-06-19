using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli
{
    public class App
    {
        private readonly string promptSymbol = ">> ";
        private readonly string splitPattern = @"""(?:(?<= "")([^""]+)""\s*)|\s*([^""\s]+)";
        private readonly ICommandDispatcher _dispatcher;
        private readonly IMessageService _messageService;

        public App(ICommandDispatcher dispatcher, IMessageService messageService)
        {
            _dispatcher = dispatcher;
            _messageService = messageService;
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
            var args = Regex.Split(command, splitPattern).Where(s => !string.IsNullOrEmpty(s.Trim()));

            var parserResult = Parser.Default.ParseArguments<SyncCommand, GetProjectTypesCommand, GetFrameworksCommand, GetPagesCommand, GetFeaturesCommand, GenerateCommand, CloseCommand>(args);

              var exitCode = parserResult.MapResult(
                    (SyncCommand opts) => DispatchCommand(opts),
                    (GetProjectTypesCommand opts) => DispatchCommand(opts),
                    (GetFrameworksCommand opts) => DispatchCommand(opts),
                    (GetPagesCommand opts) => DispatchCommand(opts),
                    (GetFeaturesCommand opts) => DispatchCommand(opts),
                    (GenerateCommand opts) => DispatchCommand(opts),
                    (CloseCommand opts) => DispatchCommand(opts),
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

        private async Task<int> DispatchCommand(ICommand command)
        {
            var validations = await _dispatcher.ValidateAsync(command);

            if(validations.IsValid)
            {
                return await _dispatcher.DispatchAsync(command);
            }

            _messageService.SendErrors(validations.Messages);
            return 0;

        }
    }
}
