using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands
{
    [Verb("generate", HelpText = "Generate user selection.")]
    public class GenerateCommand : ICommand
    {
        [Option('d', "data", Required = true, HelpText = "Generation data in json format")]
        public string GenerationDataJson { get; set; }
    }
}
