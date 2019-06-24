// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Models
{
    public class GenerationItem
    {
        [Required]
        public string TemplateId { get; set; }

        [Required]
        public string Name { get; set; }

        public UserSelectionItem ToGenerationItem() => new UserSelectionItem { Name = this.Name.MakeSafeProjectName(), TemplateId = this.TemplateId };
    }
}
