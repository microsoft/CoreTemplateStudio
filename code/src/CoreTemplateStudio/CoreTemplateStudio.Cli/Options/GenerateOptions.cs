using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("generate", HelpText = "Generate user selection.")]
    public class GenerateOptions: ICommand
    {
    }
}
