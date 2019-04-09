// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Core
{
    public static class Platforms
    {
        public const string Uwp = "Uwp";

        public const string Web = "Web";

        public static IEnumerable<string> GetAllPlatforms()
        {
            yield return Platforms.Uwp;
            yield return Platforms.Web;
        }

        public static bool IsValidPlatform(string platform)
        {
            bool isValid = false;

            foreach (string plat in GetAllPlatforms())
            {
                if (plat.Equals(platform, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
    }
}
