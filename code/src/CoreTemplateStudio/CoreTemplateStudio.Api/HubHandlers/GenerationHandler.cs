// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;

using CoreTemplateStudio.Api.Models.Generation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Templates.Api.Resources;
using Microsoft.Templates.Api.Utilities;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Api.HubHandlers
{
    public class GenerationHandler
    {
        private readonly Action<string> _messageListener;

        public GenerationHandler(Action<string> messageListener)
        {
            _messageListener = messageListener;
        }

        public async Task<ActionResult<ContextProvider>> Generate(GenerationData generationData)
        {

            if (GenContext.ToolBox == null)
            {
                throw new HubException(StringRes.BadReqNotSynced);
            }

            try
            {
                ApiGenShell shell = GenContext.ToolBox.Shell as ApiGenShell;
                shell.SetMessageEventListener(_messageListener);

                var safeProjectName = generationData.ProjectName.MakeSafeProjectName();
                var combinedPath = Path.Combine(generationData.GenPath, safeProjectName, safeProjectName);

                ContextProvider provider = new ContextProvider()
                {
                    ProjectName = generationData.ProjectName,
                    GenerationOutputPath = combinedPath,
                    DestinationPath = combinedPath,
                };

                GenContext.Current = provider;

                var userSelection = generationData.ToUserSelection();
                await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

                return provider;
            }
            catch (Exception ex)
            {
                throw new HubException($"Error generating: {ex.Message}");
            }
        }
    }
}
