using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("getframeworks", HelpText = "Get frameworks by core.")]
    public class GetFrameworksOptions : ICommand
    {
        [Option('p', "project-type", Required = true, HelpText = "Project type")]
        public string ProjectType { get; set; }
    }
}
