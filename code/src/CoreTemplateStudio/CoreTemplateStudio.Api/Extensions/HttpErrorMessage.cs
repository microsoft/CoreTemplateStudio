// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace CoreTemplateStudio.Api.Extensions
{
    public class HttpErrorMessage
    {
        public int StatusCode { get; set; }

        public string UserMessage { get; set; }

        public string[] ValidationErrors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
