using System;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Services.Contracts;

namespace Microsoft.Templates.Cli.Services
{
    public class GetTemplatesService : IGetTemplatesService
    {        
        public async Task<int> ProcessAsync(GetTemplatesOptions options)
        {
            return await Task.FromResult(0);
        }
    }
}