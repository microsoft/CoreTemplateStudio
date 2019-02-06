// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Models
{
    public class FeatureModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        private readonly HashSet<Framework> frameworks;

        public string ImagePath { get; set; } = @"/icon.png";

        public FeatureModel(Feature feature, params Framework[] supportedFrameworks)
        {
            this.Name = EnumerablesHelper.GetDisplayName(feature);
            this.Description = EnumerablesHelper.GetDescription(feature);

            this.frameworks = new HashSet<Framework>();

            foreach (Framework supportedFramework in supportedFrameworks)
            {
                this.frameworks.Add(supportedFramework);
            }
        }

        public bool HasFrameworks(HashSet<Framework> toCheckFrameworks)
        {
            return toCheckFrameworks.IsSubsetOf(this.frameworks);
        }
    }
}
