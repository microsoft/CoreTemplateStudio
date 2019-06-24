// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Templates.Cli.Commands.Contracts;

namespace Microsoft.Templates.Cli.Services.Contracts
{
    public interface IOptionsService<T>
        where T : ICommand
    {
        Task<int> ProcessAsync(T options);
    }
}
