using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands
{
    public class GetTemplatesBaseCommand : ICommand
    {
        [Option('p', "project-type", Required = true, HelpText = "Project type")]
        public string ProjectType { get; set; }

        [Option('f', "frontend-framework", Required = false, HelpText = "Frontend framework")]
        public string FrontendFramework { get; set; }

        [Option('b', "backend-framework", Required = false, HelpText = "Backend framework")]
        public string BackendFramework { get; set; }
    }
}
