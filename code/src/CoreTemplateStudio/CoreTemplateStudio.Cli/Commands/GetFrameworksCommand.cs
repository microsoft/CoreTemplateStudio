using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands
{
    [Verb("getframeworks", HelpText = "Get frameworks by core.")]
    public class GetFrameworksCommand : ICommand
    {
        [Option('p', "project-type", Required = true, HelpText = "Project type")]
        public string ProjectType { get; set; }
    }
}
