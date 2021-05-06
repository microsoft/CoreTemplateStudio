// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.AddJsonDictionaryItem
{
    public class AddJsonDictionaryItemPostAction : TemplateDefinedPostAction
    {
        public const string Id = "CB387AC0-16D0-4E07-B41A-F1EA616A7CA9";

        private readonly Dictionary<string, string> _parameters;

        private readonly string _destinationPath;

        public override Guid ActionId { get => new Guid(Id); }

        public AddJsonDictionaryItemPostAction(string relatedTemplate, IPostAction templatePostAction, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);

            var jsonPath = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["jsonPath"]));

            var keyToDict = Args["key"];

            JObject json = JObject.Parse(File.ReadAllText(jsonPath));

            var dictContent = json[keyToDict].ToObject<Dictionary<string, string>>();
            var contentToAdd = JsonConvert.DeserializeObject<Dictionary<string, string>>(Args["dict"]);

            var newContent = dictContent.Merge(contentToAdd);

            json[keyToDict] = JObject.FromObject(newContent);

            string lineEndingPattern = @"(\n)|(\r\n)";
            var originalEncoding = FileHelper.GetEncoding(jsonPath);
            var originalLineEnding = FileHelper.GetLineEnding(jsonPath);

            var jsonString = Regex.Replace(json.ToString(Formatting.Indented), lineEndingPattern, originalLineEnding);

            // TODO keep it as before for Windows
            if (originalLineEnding == FileHelper.LineEndingWindows)
            {
                File.WriteAllText(jsonPath, jsonString, originalEncoding);
            }
            else
            {
                File.WriteAllText(jsonPath, jsonString + originalLineEnding, originalEncoding);
            }
        }
    }
}
