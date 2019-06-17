using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("close", HelpText = "Close command.")]
    public class CloseOptions : ICommand
    {
    }
}
