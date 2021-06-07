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
    public class SafeRenameDirectoryTests
    {
        private DateTime _logDate;
        private string _logFile;

        [Fact]
        public void SafeRenameDirectory_ValidData_ShouldMoveDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\SafeRenameDirectory");
            var newFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject_Copy");
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
        public void SafeRenameDirectory_DoesNotExist_ShouldHandleException(string rootDir, string newRootDir)
        {
            SetupLogData();

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(rootDir, newRootDir);

            Assert.False(File.Exists(_logFile));
        }

        [Fact]
        public void SafeRenameDirectory_WrongFolder_ShouldLogException()
        {
            SetupLogData();

            var rootDir = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\");
            var newDir = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(rootDir, newDir);

            Assert.True(File.Exists(_logFile));

            CheckLoggingDataIsExpected("Warning");
        }

        [Fact]
        public void SafeRenameDirectory_WrongFolder_WarnOnFailureFalse_ShouldNotLogError()
        {
            SetupLogData();

            var rootDir = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\");
            var newDir = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(rootDir, newDir, false);

            Assert.False(File.Exists(_logFile));
        }

        // TODO Move to shared place
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

        private void CheckLoggingDataIsExpected(string errorLevel)
        {
            var logFileLines = File.ReadAllText(_logFile);

            // [2021-06-07 20:51:30.557]...  Warning Error creating folder 'C:\...\bin\Debug\netcoreapp3.1\TestData\EnsureFolderExists\Test_EnsureFolderExists': ..... because a file or directory with the same name already exists.
            Assert.Contains(errorLevel, logFileLines);
            Assert.Contains($"{_logDate.Date:yyyy-MM-dd}", logFileLines, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
