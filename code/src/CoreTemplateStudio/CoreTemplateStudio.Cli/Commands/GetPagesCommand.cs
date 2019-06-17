using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands
{
    [Verb("getpages", HelpText = "Get pages by core.")]
    public class GetPagesCommand : ICommand
    {
        [Option('p', "project-type", Required = true, HelpText = "Project type")]
        public string ProjectType { get; set; }

        [Option('f', "frontend-framework", Required = true, HelpText = "Frontend framework")]
        public string FrontendFramework { get; set; }

        [Option('b', "backend-framework", Required = true, HelpText = "Backend framework")]
        public string BackendFramework { get; set; }
    }
}
