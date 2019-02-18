using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Api.Test.ResponseModels
{
    internal class ApiResponse
    {
        public int StatusCode { get; set; }
        public IDictionary<string, object> Value { get; set; }
    }
}
