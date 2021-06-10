// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Collection("Unit Test Logs")]
    [Trait("ExecutionSet", "Minimum")]
    public class SafeDeleteDirectoryTests
    {
        private readonly LogFixture _logFixture;

        public SafeDeleteDirectoryTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void SafeDeleteDirectory_DirectoryExists_ShouldDeleteDirectory()
        {
            var sourceFolder = $"{_logFixture.TestFolderPath}\\SafeDelete";

            Directory.CreateDirectory(sourceFolder);

            var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

            Fs.SafeDeleteDirectory(sourceFolder, false);

            var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

            var directoryHasBeenDeleted = totalNewDirectories < totalOriginalDirectories;

            Assert.True(directoryHasBeenDeleted);

            // Note: no need to tidy up test as this is already removed
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SafeDeleteDirectory_DoesNotExist_ShouldNotThrowException(string rootDir)
        {
            Fs.SafeDeleteDirectory(rootDir);
        }

        [Fact]
        public void SafeDeleteDirectory_WrongFolder_ShouldNotThrowException()
        {
            var rootDir = $"{_logFixture.TestFolderPath}\\SafeDeleteDirectory";

            Fs.SafeDeleteDirectory(rootDir, false);
        }
    }
}
