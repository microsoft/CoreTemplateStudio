// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.TemplateEngine.Edge.Settings.TemplateInfoReaders;
using Microsoft.Templates.Api.Extensions;
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

            AddTemplateInfo(userSelection, obj, nameof(UserSelection.Pages));
            AddTemplateInfo(userSelection, obj, nameof(UserSelection.Features));
            return userSelection;
        }

        private void AddTemplateInfo(UserSelection selection, JObject obj, string path)
        {
            JArray items = obj.Get<JArray>(path);

            if (items == null)
            {
                return;
            }

            foreach (JObject item in items)
            {
                JObject template = item.Get<JObject>("Template");

                TemplateInfo templateInfo = new TemplateInfo
                {
                    Name = item.ToString("name").MakeSafeProjectName(),
                    Template = TemplateInfoReaderVersion1_0_0_0.FromJObject(template),
                };

                selection.Add(templateInfo);
            }
        }
    }
}
