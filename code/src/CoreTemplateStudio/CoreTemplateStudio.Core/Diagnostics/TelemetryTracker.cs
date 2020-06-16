// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryTracker : IDisposable
    {
        public TelemetryTracker()
        {
        }

        public async Task TrackWizardCompletedAsync(WizardTypeEnum wizardType, WizardActionEnum wizardAction, string vsProductVersion)
        {
            var properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.WizardStatus, WizardStatusEnum.Completed.ToString() },
                { TelemetryProperties.WizardType, wizardType.ToString() },
                { TelemetryProperties.WizardAction, wizardAction.ToString() },
                { TelemetryProperties.EventName, TelemetryEvents.Wizard },
            };

            TelemetryService.Current.SetContentVsProductVersionToContext(vsProductVersion);
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
        }

        public async Task TrackWizardCancelledAsync(WizardTypeEnum wizardType, string vsProductVersion, bool syncInProgress)
        {
            var properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.WizardStatus, WizardStatusEnum.Cancelled.ToString() },
                { TelemetryProperties.WizardType, wizardType.ToString() },
                { TelemetryProperties.EventName, TelemetryEvents.Wizard },
                { TelemetryProperties.SyncInProgress, syncInProgress.ToString() },
            };

            TelemetryService.Current.SetContentVsProductVersionToContext(vsProductVersion);

            GenContext.ToolBox.Shell.SafeTrackWizardCancelledVsTelemetry(properties);

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
        }

        public async Task TrackProjectGenAsync(ITemplateInfo template, string appProjectType, string appFx, string appPlatform, TemplateCreationResult result, Guid vsProjectId, string language, GenItemsTelemetryData genItemsTelemetryData = null, double? timeSpent = null, Dictionary<ProjectMetricsEnum, double> performanceCounters = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (template.GetTemplateType() != TemplateType.Project)
            {
                return;
            }

            GenStatusEnum telemetryStatus = result.Status == CreationResultStatus.Success ? GenStatusEnum.Completed : GenStatusEnum.Error;

            await TrackProjectAsync(telemetryStatus, template.GetTelemetryName(), appProjectType, appFx, appPlatform, vsProjectId, language, genItemsTelemetryData, timeSpent, performanceCounters, result.Status, result.Message);
        }

        public async Task TrackItemGenAsync(ITemplateInfo template, GenSourceEnum genSource, string appProjectType, string appFx, string appPlatform, TemplateCreationResult result)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (template != null && result != null)
            {
                switch (template.GetTemplateType())
                {
                    case TemplateType.Page:
                        await TrackItemGenAsync(TelemetryEvents.PageGen, template, genSource, appProjectType, appFx, appPlatform, result);
                        break;
                    case TemplateType.Feature:
                        await TrackItemGenAsync(TelemetryEvents.FeatureGen, template, genSource, appProjectType, appFx, appPlatform, result);
                        break;
                    case TemplateType.Service:
                        await TrackItemGenAsync(TelemetryEvents.ServiceGen, template, genSource, appProjectType, appFx, appPlatform, result);
                        break;
                    case TemplateType.Testing:
                        await TrackItemGenAsync(TelemetryEvents.TestingGen, template, genSource, appProjectType, appFx, appPlatform, result);
                        break;
                    case TemplateType.Unspecified:
                        break;
                }
            }
        }

        public async Task TrackNewItemAsync(TemplateType templateType, string appType, string appFx, string appPlatform, string language, Guid vsProjectId, GenItemsTelemetryData genItemsTelemetryData = null, double? timeSpent = null, CreationResultStatus genStatus = CreationResultStatus.Success, string message = "")
        {
            var itemType = templateType.GetNewItemType();
            var itemTypeString = itemType != null ? itemType.ToString() : string.Empty;

            var properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.ProjectType, appType },
                { TelemetryProperties.Framework, appFx },
                { TelemetryProperties.GenEngineStatus, genStatus.ToString() },
                { TelemetryProperties.GenEngineMessage, message },
                { TelemetryProperties.EventName, TelemetryEvents.NewItemGen },
                { TelemetryProperties.VisualStudioActiveProjectGuid, vsProjectId.ToString() },
                { TelemetryProperties.Language, language },
                { TelemetryProperties.VsProjectCategory, appPlatform },
                { TelemetryProperties.NewItemType, itemTypeString },
            };

            var metrics = new Dictionary<string, double>();

            if (genItemsTelemetryData.PagesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.PagesCount, genItemsTelemetryData.PagesCount.Value);
            }

            if (timeSpent.HasValue)
            {
                metrics.Add(TelemetryMetrics.TimeSpent, timeSpent.Value);
            }

            if (genItemsTelemetryData.FeaturesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.FeaturesCount, genItemsTelemetryData.FeaturesCount.Value);
            }

            if (genItemsTelemetryData.ServicesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.ServicesCount, genItemsTelemetryData.ServicesCount.Value);
            }

            if (genItemsTelemetryData.TestingCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.TestingCount, genItemsTelemetryData.TestingCount.Value);
            }

            GenContext.ToolBox.Shell.SafeTrackNewItemVsTelemetry(properties, genItemsTelemetryData.PageIdentities, genItemsTelemetryData.FeatureIdentities, genItemsTelemetryData.ServiceIdentities, genItemsTelemetryData.TestingIdentities, metrics);

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.NewItemGen, properties, metrics).ConfigureAwait(false);
        }

        public async Task TrackEditSummaryItemAsync(EditItemActionEnum trackedAction)
        {
            var properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.SummaryItemEditAction, trackedAction.ToString() },
                { TelemetryProperties.EventName, TelemetryEvents.EditSummaryItem },
            };

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.EditSummaryItem, properties).ConfigureAwait(false);
        }

        private async Task TrackProjectAsync(GenStatusEnum status, string templateName, string appType, string appFx, string appPlatform, Guid vsProjectId, string language, GenItemsTelemetryData genItemsTelemetryData = null, double? timeSpent = null, Dictionary<ProjectMetricsEnum, double> performanceCounters = null, CreationResultStatus genStatus = CreationResultStatus.Success, string message = "")
        {
            var properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Status, status.ToString() },
                { TelemetryProperties.ProjectType, appType },
                { TelemetryProperties.Framework, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenEngineStatus, genStatus.ToString() },
                { TelemetryProperties.GenEngineMessage, message },
                { TelemetryProperties.EventName, TelemetryEvents.ProjectGen },
                { TelemetryProperties.Language, language },
                { TelemetryProperties.VisualStudioActiveProjectGuid, vsProjectId.ToString() },
                { TelemetryProperties.VsProjectCategory, appPlatform },
            };

            var metrics = new Dictionary<string, double>();

            if (genItemsTelemetryData.PagesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.PagesCount, genItemsTelemetryData.PagesCount.Value);
            }

            if (genItemsTelemetryData.FeaturesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.FeaturesCount, genItemsTelemetryData.FeaturesCount.Value);
            }

            if (genItemsTelemetryData.ServicesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.ServicesCount, genItemsTelemetryData.ServicesCount.Value);
            }

            if (genItemsTelemetryData.TestingCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.TestingCount, genItemsTelemetryData.TestingCount.Value);
            }

            if (timeSpent.HasValue)
            {
                metrics.Add(TelemetryMetrics.TimeSpent, timeSpent.Value);
            }

            if (performanceCounters != null)
            {
                foreach (var perfCounter in performanceCounters)
                {
                    metrics.Add(TelemetryMetrics.ProjectMetricsTimeSpent + perfCounter.Key.ToString(), perfCounter.Value);
                }
            }

            GenContext.ToolBox.Shell.SafeTrackProjectVsTelemetry(properties, genItemsTelemetryData.PageIdentities, genItemsTelemetryData.FeatureIdentities, genItemsTelemetryData.ServiceIdentities, genItemsTelemetryData.TestingIdentities, metrics, status == GenStatusEnum.Completed);

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.ProjectGen, properties, metrics).ConfigureAwait(false);
        }

        private async Task TrackItemGenAsync(string eventToTrack, ITemplateInfo template, GenSourceEnum genSource, string appProjectType, string appFx, string appPlatform, TemplateCreationResult result)
        {
            GenStatusEnum telemetryStatus = result.Status == CreationResultStatus.Success ? GenStatusEnum.Completed : GenStatusEnum.Error;

            await TrackItemGenAsync(eventToTrack, telemetryStatus, appProjectType, appFx, appPlatform, template.GetTelemetryName(), genSource, result.Status, result.Message);
        }

        private async Task TrackItemGenAsync(string eventToTrack, GenStatusEnum status, string appType, string appFx, string appPlatform, string templateName, GenSourceEnum genSource, CreationResultStatus genStatus = CreationResultStatus.Success, string message = "")
        {
            var properties = new Dictionary<string, string>
            {
                { TelemetryProperties.Status, status.ToString() },
                { TelemetryProperties.Framework, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenEngineStatus, genStatus.ToString() },
                { TelemetryProperties.GenEngineMessage, message },
                { TelemetryProperties.EventName, eventToTrack },
                { TelemetryProperties.GenSource, genSource.ToString() },
                { TelemetryProperties.ProjectType, appType },
                { TelemetryProperties.VsProjectCategory, appPlatform },
            };

            await TelemetryService.Current.TrackEventAsync(eventToTrack, properties).ConfigureAwait(false);
        }

        ~TelemetryTracker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                TelemetryService.Current.Dispose();
            }

            // free native resources if any.
        }
    }
}
