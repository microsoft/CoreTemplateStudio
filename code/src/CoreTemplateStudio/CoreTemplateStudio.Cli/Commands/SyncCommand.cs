// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using CommandLine;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Commands
{
    [Verb("sync", HelpText = "Sync templates and update UI.")]
    public class SyncCommand : ICommand
    {
        public SyncCommand(string path)
        {
#if DEBUG
            Path = path;
#else
            Path =  @"..";
#endif
        }

        [Option('p', "path", Required = true, HelpText = "Sync templates path")]
        public string Path { get; }

        public string FullPath => GetFullPath();

        public string Platform => "Web";

        public string Language => "Any";

        private string GetFullPath()
        {
#if DEBUG
            return Path + "/templates";

#else
            return System.IO.Path.Combine(Path, $"{Platform}.{Language}.Templates.mstx");
#endif
        }
    }
}
