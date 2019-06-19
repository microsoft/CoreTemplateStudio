using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Utilities;
using System;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IGenerateService
    {
        Task<ContextProvider> GenerateAsync(GenerationData generationData, Action<string> messageListener);
    }
}