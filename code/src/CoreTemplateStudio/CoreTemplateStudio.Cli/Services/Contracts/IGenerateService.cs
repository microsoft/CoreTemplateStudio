// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Models;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IGenerateService
    {
        Task<GenerationResult> GenerateAsync(GenerationData generationData);

        IEnumerable<TemplateLicense> GetAllLicences(GenerationData generationData);
    }
}
