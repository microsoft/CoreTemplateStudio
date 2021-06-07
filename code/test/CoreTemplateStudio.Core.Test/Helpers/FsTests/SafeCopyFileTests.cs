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
    public class SafeCopyFileTests
    {
        private string _sourceFile;
        private DateTime _logDate;
        private string _destFolder;
        private string _logFile;

        public SafeCopyFileTests()
        {
            _sourceFile = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");
            _destFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject_Dest");
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            _destFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\SafeCopyFile_DirectoryTest_Dest");

            try
            {
                var totalOriginalDirectories = Directory.GetParent(_destFolder).GetDirectories().Length;

                Fs.SafeCopyFile(_sourceFile, _destFolder, true);

                var totalNewDirectories = Directory.GetParent(_destFolder).GetDirectories().Length;

                var directoryHasBeenCreated = totalNewDirectories > totalOriginalDirectories;

                Assert.True(directoryHasBeenCreated);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(_destFolder, true);
            }
        }

        [Fact]
        public void SafeCopyFile_FileDoesNotExist_ShouldCreateNewFileWhileCopying()
        {
            _destFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\SafeCopyFile_FileTest_Dest");
            var expectedDestinationFile = Path.Combine(_destFolder, Path.GetFileName(_sourceFile));
            Directory.CreateDirectory(_destFolder);

            var totalOriginalFiles = Directory.GetFiles(_destFolder).Length;

            try
            {
                Fs.SafeCopyFile(_sourceFile, _destFolder, true);

                var totalNewFiles = Directory.GetFiles(_destFolder).Length;

                var fileHasBeenCreated = totalNewFiles > totalOriginalFiles;
                Assert.True(fileHasBeenCreated);
                Assert.True(File.Exists(expectedDestinationFile));
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(_destFolder, true);
            }
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryAlreadyExists_ShouldNotCreateDirectory()
        {
            var totalOriginalDirectories = Directory.GetParent(_destFolder).Parent.GetDirectories().Length;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            var totalNewDirectories = Directory.GetParent(_destFolder).Parent.GetDirectories().Length;

            var noDirectoryHasBeenCreated = totalOriginalDirectories == totalNewDirectories;

            Assert.True(noDirectoryHasBeenCreated);
        }

        [Fact]
        public void SafeCopyFile_FileAlreadyExists_ShouldNotCreateNewFileWhileCopying()
        {
            var totalOriginalFiles = Directory.GetParent(_destFolder).GetFiles().Length;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            var totalNewFiles = Directory.GetParent(_destFolder).GetFiles().Length;

            var noFileHasBeenCreated = totalNewFiles == totalOriginalFiles;

            Assert.True(noFileHasBeenCreated);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SafeCopyFile_SourceFileNullOrEmpty_ShouldLogException(string filePath)
        {
            SetupLogData();

            _sourceFile = filePath;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            // TODO: review this is the expected behaviour.
            // Maybe we want to check first if the origin file exists before trying to do anything else.
            // log it as a warning?
            Assert.True(File.Exists(_logFile));

            CheckLoggingDataIsExpected("Warning");
        }

        [Fact]
        public void SafeCopyFile_CouldNotFindFile_ShouldLogException()
        {
            SetupLogData();

            _sourceFile = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\FileDoNotExists.csproj");
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            // TODO: review this is the expected behaviour.
            // Maybe we want to check first if the origin file exists before trying to do anything else.
            // log it as a warning?
            Assert.True(File.Exists(_logFile));

            CheckLoggingDataIsExpected("Warning");
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_ShouldLogException()
        {
            SetupLogData();

            // to force an exception while trying to copy a file. File without permissions instead of valid folder
            _sourceFile = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            Assert.True(File.Exists(_logFile));

            CheckLoggingDataIsExpected("Warning");
        }

        [Fact]
        public void SafeCopyFile_OverrideFalse_ShouldLogException()
        {
            SetupLogData();

            _sourceFile = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, false);

            Assert.True(File.Exists(_logFile));

            CheckLoggingDataIsExpected("Warning");
        }

        // TODO: Moved to shared place
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
