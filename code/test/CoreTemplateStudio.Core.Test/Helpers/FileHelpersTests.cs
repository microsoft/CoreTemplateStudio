// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers
{
    [Trait("ExecutionSet", "Minimum")]
    public class FileHelpersTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(null)]
        public void GetFileContent_EmptyFilePath_ShouldReturnEmpty(string sourceFile)
        {
            var actual = FileHelper.GetFileContent(sourceFile);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("TestData\\TestProject\\")]
        [InlineData("TestData\\TestProject\\TestNotExisting.csproj")]
        public void GetFileContent_WrongFilePath_ShouldReturnEmpty(string file)
        {
            var sourceFile = Path.Combine(Environment.CurrentDirectory, file);

            var actual = FileHelper.GetFileContent(sourceFile);

            Assert.Empty(actual);
        }

        [Fact]
        public void GetFileContent_FileExists_ShouldReturnsCorrectly()
        {
            var sourceFile = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");

            var actual = FileHelper.GetFileContent(sourceFile);

            Assert.NotEmpty(actual);
        }

        [Fact(Skip = "try if this breaks the build pipeline due to running")]
        public void GetFileContent_WithFileOpened_ShouldReturnEmpty()
        {
            var sourceFile = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");

            using (StreamWriter file = new StreamWriter(sourceFile))
            {
                var actual = FileHelper.GetFileContent(sourceFile);

                Assert.Empty(actual);
                file.Close();
            }
        }
    }
}
