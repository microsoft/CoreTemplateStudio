using CommandLine;

namespace Microsoft.Templates.Cli.Options
{
    [Verb("sync", HelpText = "Sync templates and update UI.")]
    public class SyncOptions : IOptions
    {
        [Option('p', "path", Required = true, HelpText = "Sync templates path")]
        public string Path { get; set; }
    }
}
