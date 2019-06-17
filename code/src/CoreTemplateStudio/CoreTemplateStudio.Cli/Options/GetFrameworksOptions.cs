using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("getframeworks", HelpText = "Get frameworks by core.")]
    public class GetFrameworksOptions :IOptions
    {
        [Option('p', "project-type", Required = true, HelpText = "Project type")]
        public string ProjectType { get; set; }
    }
}
