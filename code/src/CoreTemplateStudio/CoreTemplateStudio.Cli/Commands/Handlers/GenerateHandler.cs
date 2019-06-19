using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GenerateHandler : ICommandHandler<GenerateCommand>
    {
        private readonly IMessageService _messageService;
        private readonly IGenerateService _generateService;

        public GenerateHandler(IMessageService messageService, IGenerateService generateService)
        {
            _messageService = messageService;
            _generateService = generateService;
        }


        public async Task<int> ExecuteAsync(GenerateCommand command)
        {
            var generationData = JsonConvert.DeserializeObject<GenerationData>(command.GenerationDataJson);
            await _generateService.GenerateAsync(generationData, OnGetMessage);

            return 0;
        }

        private void OnGetMessage(string message) => _messageService.SendMessage(message);
    }
}
