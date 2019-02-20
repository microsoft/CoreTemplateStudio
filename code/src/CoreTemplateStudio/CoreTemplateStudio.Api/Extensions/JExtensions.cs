// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Microsoft.Templates.Api
{
    internal static class JExtensions
    {
        public static string ToString(this JToken token, string key)
        {
            if (key == null)
            {
                if (token == null || token.Type != JTokenType.String)
                {
                    return null;
                }

                return token.ToString();
            }

            if (!(token is JObject obj))
            {
                return null;
            }

            if (!obj.TryGetValue(key, StringComparison.OrdinalIgnoreCase, out JToken element) || element.Type != JTokenType.String)
            {
                return null;
            }

            return element.ToString();
        }

        public static Guid ToGuid(this JToken token, string key = null, Guid defaultValue = default(Guid))
        {
            string val = token.ToString(key);

            if (val == null || !Guid.TryParse(val, out Guid result))
            {
                return defaultValue;
            }

            return result;
        }

        public static T Get<T>(this JToken token, string key)
            where T : JToken
        {
            if (!(token is JObject obj))
            {
                return default(T);
            }

            if (!obj.TryGetValue(key, StringComparison.OrdinalIgnoreCase, out JToken res))
            {
                return default(T);
            }

            return res as T;
        }
    }
}
