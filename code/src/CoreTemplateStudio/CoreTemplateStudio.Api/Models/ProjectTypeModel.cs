// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Models
{
    public class ProjectTypeModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; } = @"/icon.png";

        private readonly Platform platform;

        private readonly Language language;

        public ProjectTypeModel(ProjectType type, Platform platform, Language language = Language.Any)
        {
            this.Name = EnumerablesHelper.GetDisplayName(type);
            this.Description = EnumerablesHelper.GetDescription(type);

            this.platform = platform;
            this.language = language;
        }

        public bool IsPlatform(Platform pform)
        {
            return this.platform.Equals(pform);
        }

        public bool IsLanguage(Language lang)
        {
            return this.language.Equals(lang);
        }
    }
}
