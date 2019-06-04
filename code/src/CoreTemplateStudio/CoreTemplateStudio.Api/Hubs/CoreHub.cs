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
        public async Task<ActionResult<SyncModel>> SyncTemplates(string path)
        {
            var handler = new SyncHandler(path, SendMessageToClient);

            return await handler.Sync();
        }

        public async Task<ActionResult<ContextProvider>> Generate(GenerationData generationData)
        {
            var handler = new GenerationHandler(SendProgressToClient);

            return await handler.Generate(generationData);
        }

        private void SendMessageToClient(SyncStatus status, int progress)
        {
            Clients.Caller.SendAsync("syncMessage", status.ToString(), progress);
        }

        private void SendProgressToClient(string message)
        {
            Clients.Caller.SendAsync("genMessage", message);
        }
    }
}
