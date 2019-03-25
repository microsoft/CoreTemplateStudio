// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;

using CoreTemplateStudio.Api.Extensions.Filters;
using CoreTemplateStudio.Api.Models.Generation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Templates.Api.HubHandlers;
using Microsoft.Templates.Api.Models;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Api.Hubs
{
    public class CoreHub : Hub
    {
        public async Task<ActionResult<SyncModel>> SyncTemplates(string platform, string path, string language = ProgrammingLanguages.Any)
        {
            SyncHandler handler = new SyncHandler(platform, path, language, SendMessageToClient);

            return await handler.AttemptSync();
        }

        public async Task<ActionResult<ContextProvider>> Generate(GenerationData generationData)
        {
            ApiGenShell shell = GenContext.ToolBox.Shell as ApiGenShell;
            shell.SetMessageEventListener(SendProgressToClient);

            var combinedPath = Path.Combine(generationData.GenPath, generationData.ProjectName.MakeSafeProjectName());

            ContextProvider provider = new ContextProvider()
            {
                ProjectName = generationData.ProjectName,
                GenerationOutputPath = combinedPath,
                DestinationPath = combinedPath,
            };

            GenContext.Current = provider;

            var userSelection = generationData.ToUserSelection();
            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            // TODO: We should generationOutputPath??
            return provider;
        }

        private void SendMessageToClient(SyncStatus status, int progress)
        {
            Clients.Caller.SendAsync("syncMessage", status.ToString(), progress);
        }

        private void SendProgressToClient(object sender, string message)
        {
            Clients.Caller.SendAsync("genMessage", message);
        }
    }
}
