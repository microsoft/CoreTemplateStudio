// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Utilities
{
    public delegate void StatusBarMessageHandler(string message);

    public class CliGenShell : GenShell
    {
        private readonly IMessageService _messageService;

        public CliGenShell(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public override void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
        }

        public override void CancelWizard(bool back = true)
        {
        }

        public override void CloseSolution()
        {
        }

        public override string CreateCertificate(string publisherName)
        {
            return CliCertificateService.Instance.CreateCertificate(publisherName);
        }

        public override bool GetActiveProjectIsWts()
        {
            return true;
        }

        public override string GetActiveProjectLanguage()
        {
            return string.Empty;
        }

        public override string GetActiveProjectName()
        {
            return string.Empty;
        }

        public override string GetActiveProjectNamespace()
        {
            return string.Empty;
        }

        public override string GetActiveProjectPath()
        {
            return string.Empty;
        }

        public override string GetActiveProjectTypeGuids()
        {
            return Guid.Empty.ToString();
        }

        public override Guid GetProjectGuidByName(string projectName)
        {
            return Guid.Empty;
        }

        public override VSTelemetryInfo GetVSTelemetryInfo()
        {
            return new VSTelemetryInfo()
            {
                OptedIn = true,
                VisualStudioCulture = string.Empty,
                VisualStudioEdition = string.Empty,
                VisualStudioExeVersion = string.Empty,
                VisualStudioManifestId = string.Empty,
            };
        }

        public override bool IsBuildInProgress()
        {
            return false;
        }

        public override bool IsDebuggerEnabled()
        {
            return false;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
        }

        public override void OpenProjectOverview()
        {
        }

        public override void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
        }

        public override void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid)
        {
        }

        public override void ShowModal(IWindow shell)
        {
        }

        public override void ShowStatusBarMessage(string message)
        {
            _messageService.SendResult(MessageType.GenerateProgress, message);
        }

        public override void ShowTaskList()
        {
        }

        public override void WriteOutput(string data)
        {
        }
    }
}
