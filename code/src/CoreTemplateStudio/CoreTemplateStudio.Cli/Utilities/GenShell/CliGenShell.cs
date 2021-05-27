// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Cli.Utilities.GenShell
{
    public delegate void StatusBarMessageHandler(string message);

    public class CliGenShell : IGenShell
    {
        public CliGenShell(IMessageService messageService)
        {
            Project = new CliGenShellProject();
            Solution = new CliGenShellSolution();
            Telemetry = new CliGenShellTelemetry();
            UI = new CliGenShellUI(messageService);
            VisualStudio = new CliGenShellVisualStudio();
            Certificate = new CliGenShellCertificate();
        }

        public IGenShellProject Project { get; }

        public IGenShellSolution Solution { get; }

        public IGenShellTelemetry Telemetry { get; }

        public IGenShellUI UI { get; }

        public IGenShellVisualStudio VisualStudio { get; }

        public IGenShellCertificate Certificate { get; }
    }
}
