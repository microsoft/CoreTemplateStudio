// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Core.Gen
{
    public class UserSelectionContext
    {
        public string ProjectType { get; set; }

        public string FrontEndFramework { get; set; }

        public string BackEndFramework { get; set; }

        public string Platform { get; set; }

        public string AppModel { get; set; }

        public string Language { get; private set; }

        public UserSelectionContext(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException(nameof(language));
            }

            Language = language;
        }

        public UserSelectionContext(string language, string platform)
           : this(language)
        {
            if (string.IsNullOrWhiteSpace(platform))
            {
                throw new ArgumentNullException(nameof(platform));
            }

            Platform = platform;
        }

        public UserSelectionContext(string language, string platform, string appModel)
            : this(language, platform)
        {
            AppModel = appModel;
        }

        public UserSelectionContext(string language, string platform, string appModel, string projecttype, string frontendframework, string backendframework)
            : this(language, platform, appModel)
        {
            ProjectType = projecttype;
            FrontEndFramework = frontendframework;
            BackEndFramework = backendframework;
        }
    }
}
