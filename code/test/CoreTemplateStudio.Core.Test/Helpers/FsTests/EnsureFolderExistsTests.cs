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
    public class EnsureFolderExistsTests
    {
        private string _logFile;
        private DateTime _logDate;

        [Fact]
        public void EnsureFolderExists_DirectoryDoesNotExists_ShouldCreateDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject_EnsureFolderExists");
            try
            {
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.EnsureFolderExists(sourceFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Assert.True(totalNewDirectories > totalOriginalDirectories);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(sourceFolder);
            }
        }

        [Fact]
        public void EnsureFolderExists_DirectoryAlreadyExists_ShouldNotCreateDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\EnsureFolderExists");

            try
            {
                Directory.CreateDirectory(sourceFolder);
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.EnsureFolderExists(sourceFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Assert.True(totalOriginalDirectories == totalNewDirectories);
            }
            finally
            {
                // tidy up test data
                Directory.Delete(sourceFolder);
            }
        }

        [Fact]
        public void EnsureFolderExists_ErrorCreatingDirectory_ShouldLogException()
        {
            SetupLogData();

            // To force an error creating a Directory
            // we create a file with the name of the folder that we want to create
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\EnsureFolderExists");
            Directory.CreateDirectory(sourceFolder);
            var folderPath = Path.Combine(Environment.CurrentDirectory, sourceFolder, "Test_EnsureFolderExists");
            if (File.Exists(_logFile))
            {
                File.Delete(_logFile);
            }

            try
            {
                using var stream = File.Create(folderPath);
                _logDate = DateTime.Now;
                Fs.EnsureFolderExists(folderPath);

                Assert.True(File.Exists(_logFile));

                var logFileLines = File.ReadAllText(_logFile);

                CheckLoggingDataIsExpected("Warning");
            }
            finally
            {
                // tidy up testing data
                File.Delete(folderPath);
            }
        }

        // TODO: Move to shared place
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
