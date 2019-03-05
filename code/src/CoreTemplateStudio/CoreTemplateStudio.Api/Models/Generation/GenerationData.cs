// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace CoreTemplateStudio.Api.Models.Generation
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
        public IEnumerable<GenerationItem> Pages { get; set; }

        [Required]
        public IEnumerable<GenerationItem> Features { get; set; }

        public UserSelection ToUserSelection()
        {
            var userSelection = new UserSelection(ProjectType, FrontendFramework, BackendFramework, Platform, Language);
            userSelection.HomeName = HomeName;

            var pageTemplates = GetPageTemplates();
            userSelection.Add(pageTemplates);

            var featureTemplates = GetFeatureTemplates();
            userSelection.Add(featureTemplates);

            return userSelection;
        }

        private IEnumerable<TemplateInfo> GetPageTemplates()
        {
            var pageTemplates = GenComposer.GetPages(
                                    ProjectType,
                                    Platform,
                                    FrontendFramework,
                                    BackendFramework);

            return GetTemplateInfos(pageTemplates, Pages);
        }

        private IEnumerable<TemplateInfo> GetFeatureTemplates()
        {
            var featureTemplates = GenComposer.GetFeatures(
                                    ProjectType,
                                    Platform,
                                    FrontendFramework,
                                    BackendFramework);

            return GetTemplateInfos(featureTemplates, Features);
        }

        private IEnumerable<TemplateInfo> GetTemplateInfos(IEnumerable<ITemplateInfo> templates, IEnumerable<GenerationItem> items)
        {
            foreach (var item in items)
            {
                var template = templates.FirstOrDefault(t => t.Identity.Equals(item.Template, StringComparison.OrdinalIgnoreCase));

                if (template != null)
                {
                    yield return new TemplateInfo
                    {
                        Name = item.Name.MakeSafeProjectName(),
                        Template = template,
                    };
                }
            }
        }
    }
}
