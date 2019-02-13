// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Models
{
    public class GenerateModel
    {
        public Platform Platform { get; set; }

       /* public ProjectType ProjectType { get; set; }*/

        public Framework? Frontend { get; set; }

        public Framework? Backend { get; set; }

        public List<Page> Pages { get; set; }

        public List<Feature> Features { get; set; }

        public (bool, string) Validate()
        {
            if (Frontend != null && (Pages == null || Pages.Count == 0))
            {
                return (false, "specify at least one page if frontend framework is specified");
            }

            return (true, string.Empty);
        }

        public bool Generate()
        {
            return true;
        }
    }
}
