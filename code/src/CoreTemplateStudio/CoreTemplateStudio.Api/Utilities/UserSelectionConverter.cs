// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Api.Extensions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

using Newtonsoft.Json.Linq;

namespace Microsoft.Templates.Api.Utilities
{
    public class UserSelectionConverter
    {
        public static UserSelection FromJObject(JObject obj)
        {
            UserSelectionConverter converter = new UserSelectionConverter();

            return converter.Convert(obj);
        }

        public UserSelection Convert(JObject obj)
        {
            string projectType = obj.ToString(nameof(UserSelection.ProjectType));
            string frontEndFramework = obj.ToString(nameof(UserSelection.FrontEndFramework));
            string backEndFramework = obj.ToString(nameof(UserSelection.BackEndFramework));
            string platform = obj.ToString(nameof(UserSelection.Platform));
            string language = obj.ToString(nameof(UserSelection.Language));

            UserSelection userSelection = new UserSelection(projectType, frontEndFramework, backEndFramework, platform, language)
            {
                HomeName = obj.ToString(nameof(UserSelection.HomeName)),
            };

            AddTemplateInfo(userSelection, obj, nameof(UserSelection.Pages), TemplateType.Page);
            AddTemplateInfo(userSelection, obj, nameof(UserSelection.Features), TemplateType.Feature);
            return userSelection;
        }

        private void AddTemplateInfo(UserSelection selection, JObject obj, string path, TemplateType type)
        {
            JArray items = obj.Get<JArray>(path);

            if (items == null)
            {
                return;
            }

            List<ITemplateInfo> allTemplatesOfType = new List<ITemplateInfo>();

            if (type == TemplateType.Page)
            {
                allTemplatesOfType = GenComposer.GetPages(
                                        selection.ProjectType,
                                        selection.Platform,
                                        selection.FrontEndFramework,
                                        selection.BackEndFramework).ToList();
            }
            else if (type == TemplateType.Feature)
            {
                allTemplatesOfType = GenComposer.GetFeatures(
                                        selection.ProjectType,
                                        selection.Platform,
                                        selection.FrontEndFramework,
                                        selection.BackEndFramework).ToList();
            }
            else
            {
                return;
            }

            foreach (JObject item in items)
            {
                string templateName = item.ToString("Template");

                ITemplateInfo template = allTemplatesOfType.Where(t => t.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase))
                                                           .First();
                if (template == null)
                {
                    continue;
                }

                TemplateInfo templateInfo = new TemplateInfo
                {
                    Name = item.ToString("name").MakeSafeProjectName(),
                    Template = template,
                };

                selection.Add(templateInfo);
            }
        }
    }
}
