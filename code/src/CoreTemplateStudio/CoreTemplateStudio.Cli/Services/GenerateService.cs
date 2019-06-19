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
        public async Task<ContextProvider> GenerateAsync(GenerationData generationData, Action<string> messageListener)
        {
            if (GenContext.ToolBox == null)
            {
                throw new Exception(StringRes.BadReqNotSynced);
            }

            try
            {
                CliGenShell shell = GenContext.ToolBox.Shell as CliGenShell;
                shell.SetMessageEventListener(messageListener);

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