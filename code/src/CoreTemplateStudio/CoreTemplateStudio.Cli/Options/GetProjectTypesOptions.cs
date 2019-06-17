using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("getprojecttypes", HelpText = "Get project types.")]
    public class GetProjectTypesOptions : ICommand
    {
    }
}
