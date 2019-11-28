// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Naming;

namespace Microsoft.Templates.Cli.Models
{
    public class SyncModel
    {
        public bool WasUpdated { get; set; }

        public string TemplatesVersion { get; set; }

        public ProjectNameValidationConfig ProjectNameValidationConfig { get; set; }

        public ItemNameValidationConfig ItemNameValidationConfig { get; set; }
    }
}
