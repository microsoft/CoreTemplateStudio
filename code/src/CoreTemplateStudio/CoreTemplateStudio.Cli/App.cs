// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Cli
{
    public class App : IDisposable
    {
        private readonly string splitPattern = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private readonly ICommandDispatcher _dispatcher;
        private readonly IMessageService _messageService;
        private readonly Parser _parser;
        private int _catchedExceptionsCounter;
        private const int ExceptionsAllowed = 50;

        public App(ICommandDispatcher dispatcher, IMessageService messageService)
        {
            _dispatcher = dispatcher;
            _messageService = messageService;
            _parser = new Parser(settings => settings.AutoHelp = false);
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                isRunning = ProcessCommand(Console.ReadLine());
            }
        }

        private bool ProcessCommand(string command)
        {
            try
            {
                AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.ReceivedCommand, command)).FireAndForget();

                var args = Regex.Split(command, splitPattern)
                    .Select(s => s.Trim('"'));

                var parserResult = _parser.ParseArguments<
                        SyncCommand,
                        GetProjectTypesCommand,
                        GetFrameworksCommand,
                        GetLayoutsCommand,
                        GetPagesCommand,
                        GetFeaturesCommand,
                        GetServicesCommand,
                        GetTestingsCommand,
                        GetAllLicencesCommand,
                        GenerateCommand,
                        CloseCommand>(args);

                var exitCode = parserResult.MapResult(
                        (SyncCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetProjectTypesCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetFrameworksCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetLayoutsCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetPagesCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetFeaturesCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetServicesCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetTestingsCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GetAllLicencesCommand opts) => _dispatcher.DispatchAsync(opts),
                        (GenerateCommand opts) => _dispatcher.DispatchAsync(opts),
                        (CloseCommand opts) => Task.FromResult(1),
                        errors =>
                        {
                            var helpText = HelpText.AutoBuild(parserResult);
                            var errorText = string.Format(StringRes.ErrorParsingCommand, command, helpText);
                            _messageService.SendError(errorText);
                            AppHealth.Current.Error.TrackAsync(errorText).FireAndForget();
                            return Task.FromResult(0);
                        });

                _catchedExceptionsCounter = 0;

                // todo: use async task
                return exitCode.Result == 0;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(StringRes.ErrorExecutingCommand, command, ex.Message);
                AppHealth.Current.Exception.TrackAsync(ex, errorMessage).FireAndForget();
                _messageService.SendError(errorMessage);

                _catchedExceptionsCounter++;
                if (_catchedExceptionsCounter == ExceptionsAllowed)
                {
                    _messageService.SendError(StringRes.ErrorMaxExceptionAllowed);
                    AppHealth.Current.Error.TrackAsync(StringRes.ErrorMaxExceptionAllowed).FireAndForget();
                    return false;
                }

                return true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _parser.Dispose();
            }
        }
    }
}
