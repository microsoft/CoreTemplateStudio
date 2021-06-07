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
    public class SafeDeleteFileTests
    {
        private readonly string _filePath;
        private readonly string _dirPath;

        public SafeDeleteFileTests()
        {
            _dirPath = "TestData\\SafeDeleteFile";
            _filePath = Path.Combine(Environment.CurrentDirectory, _dirPath, "Test.csproj");
        }

        [Fact]
        public void SafeDeleteFile_ExistingFile_ShouldHaveDeletedFile()
        {
            try
            {
                Directory.CreateDirectory(_dirPath);
                using (var stream = File.Create(_filePath))
                {
                }

                Fs.SafeDeleteFile(_filePath);

                Assert.False(File.Exists(_filePath));
            }
            finally
            {
                // tidy up testing data
                File.Delete(_filePath);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SafeDeleteFile_DoesNotExist_ShouldHandleException(string filePath)
        {
            Fs.SafeDeleteFile(filePath);
        }
    }
}
