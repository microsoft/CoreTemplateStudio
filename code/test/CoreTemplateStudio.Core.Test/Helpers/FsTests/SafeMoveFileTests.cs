// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Trait("ExecutionSet", "Minimum")]
    public class SafeMoveFileTests
    {
        private string _sourceFile;
        private string _destFolder;

        public SafeMoveFileTests()
        {
            _sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Test.csproj");
            _destFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\SafeMoveFile_Dest\\");
        }
    }
}
