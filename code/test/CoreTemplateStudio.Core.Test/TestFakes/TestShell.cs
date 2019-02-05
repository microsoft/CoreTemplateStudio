using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Test.TestFakes
{
    class TestShell : GenShell
    {
        private string _platform;
        private string _language;
        
        public TestShell(string platform, string language)
        {
            _platform = platform;
            _language = language;
        }
        public override void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
            throw new NotImplementedException();
        }

        public override void CancelWizard(bool back = true)
        {
            throw new NotImplementedException();
        }

        public override void CloseSolution()
        {
            throw new NotImplementedException();
        }

        public override string CreateCertificate(string publisherName)
        {
            return TestCertificateService.Instance.CreateCertificate(publisherName);
        }

        public override string GetActiveProjectGuid()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveProjectLanguage()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveProjectName()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveProjectNamespace()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveProjectPath()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveProjectTypeGuids()
        {
            throw new NotImplementedException();
        }

        public override Guid GetVsProjectId()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override bool IsDebuggerEnabled()
        {
            throw new NotImplementedException();
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
            throw new NotImplementedException();
        }

        public override void OpenProjectOverview()
        {
            throw new NotImplementedException();
        }

        public override void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
        }

        public override void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid)
        {
            throw new NotImplementedException();
        }

        public override void ShowModal(IWindow shell)
        {
            throw new NotImplementedException();
        }

        public override void ShowStatusBarMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void ShowTaskList()
        {
            throw new NotImplementedException();
        }

        public override void WriteOutput(string data)
        {
            Console.WriteLine(data);
        }
    }
}
