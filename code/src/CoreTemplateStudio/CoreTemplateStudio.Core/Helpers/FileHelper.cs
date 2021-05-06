// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.Helpers
{
    public static class FileHelper
    {
        public static string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Encoding GetEncoding(string originalFilePath)
        {
            // Will read the file, and look at the BOM to check the encoding.
            using (var reader = new StreamReader(File.OpenRead(originalFilePath), true))
            {
                var bytes = File.ReadAllBytes(originalFilePath);
                var encoding = reader.CurrentEncoding;

                // The preamble is the first couple of bytes that may be appended to define an encoding.
                var preamble = encoding.GetPreamble();

                // We preserve the read encoding unless there is no BOM, if it is UTF-8 we return the non BOM encoding.
                if (preamble == null || preamble.Length == 0 || preamble.Where((p, i) => p != bytes[i]).Any())
                {
                    if (encoding.EncodingName == Encoding.UTF8.EncodingName)
                    {
                        return new UTF8Encoding(false);
                    }
                }

                return encoding;
            }
        }

        public const string LineEndingWindows = "\r\n";
        public const string LineEndingUnix = "\n";

        public static string GetLineEnding(string originalFilePath)
        {
            // Will read the file, and check last file ending.
            using (var reader = new StreamReader(File.OpenRead(originalFilePath), true))
            {
                var fileContent = File.ReadAllText(originalFilePath);
                string pattern = $"({LineEndingUnix})|({LineEndingWindows})";
                var lineEnding = Regex.Match(fileContent, @pattern, RegexOptions.RightToLeft);

                return lineEnding.Value;
            }
        }
    }
}
