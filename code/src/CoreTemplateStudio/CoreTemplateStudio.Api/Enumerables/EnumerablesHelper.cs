// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Reflection;

namespace CoreTemplateStudio.Api.Enumerables
{
    public static class EnumerablesHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetDisplayName(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayNameAttribute[] attributes =
                (DisplayNameAttribute[])fi.GetCustomAttributes(
                typeof(DisplayNameAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].DisplayName;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
