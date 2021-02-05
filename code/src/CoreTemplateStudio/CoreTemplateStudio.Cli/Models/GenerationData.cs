// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Cli.Models
{
    public class GenerationData
    {
        [Required(ErrorMessage = "Invalid project name")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Invalid generation path")]
        public string GenPath { get; set; }

        [Required]
        public string ProjectType { get; set; }

        [Required]
        public string FrontendFramework { get; set; }

        [Required]
        public string BackendFramework { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string HomeName { get; set; }

        [Required]
        public string Platform { get; set; }

        [Required]
        public List<GenerationItem> Pages { get; set; }

        [Required]
        public List<GenerationItem> Features { get; set; }

        public UserSelection ToUserSelection()
        {
            var context = new UserSelectionContext(Language, Platform, string.Empty, ProjectType, FrontendFramework, BackendFramework);
            var userSelection = new UserSelection(context);

            Pages.ForEach(p => userSelection.Pages.Add(p.ToGenerationItem()));
            Features.ForEach(p => userSelection.Features.Add(p.ToGenerationItem()));

            bool pagesExist = userSelection.Pages.Count > 0;

            userSelection.HomeName = pagesExist ? userSelection.Pages.FirstOrDefault().Name
                                                : string.Empty;

            return userSelection;
        }
    }
}
