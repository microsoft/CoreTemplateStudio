using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("sync", HelpText = "Sync templates and update UI.")]
    public class SyncOptions : ICommand
    {
        [Option('p', "path", Required = true, HelpText = "Sync templates path")]
        public string Path { get; set; }
    }
}
