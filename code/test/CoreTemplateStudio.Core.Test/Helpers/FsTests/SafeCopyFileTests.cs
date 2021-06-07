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
        private string _destFolder;

        public SafeCopyFileTests()
        {
            _sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Test.csproj");
            _destFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject_Dest");
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            _destFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\SafeCopyFile_DirectoryTest_Dest");

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
            _destFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\SafeCopyFile_FileTest_Dest");
            var expectedDestinationFile = Path.Combine(_destFolder, Path.GetFileName(_sourceFile));
            Directory.CreateDirectory(_destFolder);

            var totalOriginalFiles = Directory.GetFiles(_destFolder).Length;

            try
            {
                Fs.SafeCopyFile(_sourceFile, _destFolder, true);

                var totalNewFiles = Directory.GetFiles(_destFolder).Length;

                var fileHasBeenCreated = totalNewFiles > totalOriginalFiles;
                Assert.True(fileHasBeenCreated);
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
        public void SafeCopyFile_SourceFileNullOrEmpty_ShouldHandleException(string filePath)
        {
            _sourceFile = filePath;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);
        }

        [Fact]
        public void SafeCopyFile_CouldNotFindFile_ShouldHandleException()
        {
            _sourceFile = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\FileDoNotExists.csproj");

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_ShouldHandleException()
        {
            // to force an exception while trying to copy a file. File without permissions instead of valid folder
            _sourceFile = Environment.CurrentDirectory;

            Fs.SafeCopyFile(_sourceFile, _destFolder, true);
        }

        [Fact]
        public void SafeCopyFile_OverrideFalse_ShouldHandleException()
        {
            _sourceFile = Environment.CurrentDirectory;

            Fs.SafeCopyFile(_sourceFile, _destFolder, false);
        }
    }
}
