// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Core;
using Newtonsoft.Json;

namespace Microsoft.Templates.Cli.Commands.Handlers
{
    public class GetAllLicencesHandler : ICommandHandler<GetAllLicencesCommand>
    {
        private readonly IMessageService _messageService;
        private readonly IGenerateService _generateService;

        public GetAllLicencesHandler(IMessageService messageService, IGenerateService generateService)
        {
            _messageService = messageService;
            _generateService = generateService;
        }

        public async Task<int> ExecuteAsync(GetAllLicencesCommand command)
        {
            var data = string.Join(' ', command.GenerationDataJson);
            var generationData = JsonConvert.DeserializeObject<GenerationData>(data);
            var licences = _generateService.GetAllLicences(generationData);

            _messageService.SendResult(MessageType.GetAllLicencesResult, licences);
            return await Task.FromResult(0);
        }
    }
}
