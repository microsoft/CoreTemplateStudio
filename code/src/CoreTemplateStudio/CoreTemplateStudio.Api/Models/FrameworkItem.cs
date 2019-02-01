// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Models
{
    public class FrameworkItem
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string FrameworkType { get; set; }

        public string ImagePath { get; set; } = @"C:\Some\Dummy\Path";

        public HashSet<ShortProjectType> ProjectTypes { get; internal set; }

        public FrameworkItem(ShortFramework framework, FrameworkType frameworkType, params ShortProjectType[] projects)
        {
            this.Name = EnumerablesHelper.GetDisplayName(framework);
            this.Description = EnumerablesHelper.GetDescription(framework);
            this.FrameworkType = EnumerablesHelper.GetDisplayName(frameworkType);

            this.ProjectTypes = new HashSet<ShortProjectType>();

            foreach (ShortProjectType spt in projects)
            {
                this.ProjectTypes.Add(spt);
            }
        }
    }
}
