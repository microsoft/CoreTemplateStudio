// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Services
{
    public class GenerateService : IGenerateService
    {
        public async Task<ContextProvider> GenerateAsync(GenerationData generationData)
        {
            try
            {
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
                throw new Exception(string.Format(StringRes.ErrorGenerating, ex.Message));
            }
        }
    }
}
