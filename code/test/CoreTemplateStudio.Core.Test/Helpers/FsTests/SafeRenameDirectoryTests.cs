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
    public class SafeRenameDirectoryTests
    {
        private readonly LogFixture _logFixture;
        private DateTime _logDate;

        private const string ErrorMessage = " can't be rename";
        private const string ErrorLevel = "Warning";

        public SafeRenameDirectoryTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void SafeRenameDirectory_ValidData_ShouldMoveDirectory()
        {
            var sourceFolder = $"{_logFixture.TestFolderPath}\\SafeRenameDirectory";
            var newFolder = $"{_logFixture.TestFolderPath}\\TestProject_Copy";
            Directory.CreateDirectory(sourceFolder);
            try
            {
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.SafeRenameDirectory(sourceFolder, newFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                var sameNumberOfDirectories = totalNewDirectories == totalOriginalDirectories;
                Assert.True(sameNumberOfDirectories);
                var oldDirectoryHasBeenMovedToNewDirectory = Directory.Exists(newFolder) && !Directory.Exists(sourceFolder);
                Assert.True(oldDirectoryHasBeenMovedToNewDirectory);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(newFolder, true);
            }
        }

        [Theory]
        [InlineData("", "anything")]
        [InlineData(null, "anything")]
        public void SafeRenameDirectory_DoesNotExist_ShouldNotThrowException(string rootDir, string newRootDir)
        {
            // TODO HERE: this doesn´t check anything!!
            Fs.SafeRenameDirectory(rootDir, newRootDir);
        }
    }
}
