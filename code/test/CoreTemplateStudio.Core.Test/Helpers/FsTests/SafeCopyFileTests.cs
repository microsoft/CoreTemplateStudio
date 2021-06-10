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
    public class SafeCopyFileTests
    {
        private readonly LogFixture _logFixture;

        private string _sourceFile;
        private DateTime _logDate;
        private string _destFolder;
        private const string ErrorMessage = "can't be copied to";
        private const string ErrorLevel = "Warning";

        public SafeCopyFileTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            _sourceFile = $"TestData\\TestProject\\Test.csproj";
            _destFolder = $"{_logFixture.TestFolderPath}\\TestProject_Dest";
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            _destFolder = $"{_logFixture.TestFolderPath}\\SafeCopyFile_DirectoryTest_Dest";

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
            _destFolder = $"{_logFixture.TestFolderPath}\\SafeCopyFile_FileTest_Dest";
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
            var totalOriginalDirectories = Directory.GetParent(_destFolder).GetDirectories().Length;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            var totalNewDirectories = Directory.GetParent(_destFolder).GetDirectories().Length;

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
            _sourceFile = filePath;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_CouldNotFindFile_ShouldLogException()
        {
            _sourceFile = $"{_logFixture.TestFolderPath}\\TestProject\\FileDoNotExists.csproj";
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_ShouldLogException()
        {
            // to force an exception while trying to copy a file. File without permissions instead of valid folder
            _sourceFile = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_OvewriteFalse_ShouldLogException()
        {
            _sourceFile = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(_sourceFile, _destFolder, false);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }
    }
}
