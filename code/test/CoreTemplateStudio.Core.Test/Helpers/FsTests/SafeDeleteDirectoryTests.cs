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
    public class SafeDeleteDirectoryTests
    {
        private string _logFile;

        [Fact]
        public void SafeDeleteDirectory_DirectoryExists_ShouldDeleteDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\SafeDelete");

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
        public void SafeDeleteDirectory_DoesNotExist_ShouldNotError(string rootDir)
        {
            SetupLogData();

            Fs.SafeDeleteDirectory(rootDir);

            Assert.False(File.Exists(_logFile));
        }

        [Fact]
        public void SafeDeleteDirectory_WrongFolder_ShouldNotError()
        {
            SetupLogData();

            var rootDir = Path.Combine(Environment.CurrentDirectory, "TestData\\SafeDeleteDirectory");

            Fs.SafeDeleteDirectory(rootDir, false);

            Assert.False(File.Exists(_logFile));
        }

        // TODO: Shared somewhere.
        private void SetupLogData()
        {
            _logFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Configuration.Current.LogFileFolderPath,
                $"WTS_{Configuration.Current.Environment}_{DateTime.Now:yyyyMMdd}.log");

            if (File.Exists(_logFile))
            {
                File.Delete(_logFile);
            }
        }
    }
}
