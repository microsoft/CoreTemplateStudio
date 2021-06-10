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
    public class SafeMoveFileTests
    {
        private readonly LogFixture _logFixture;
        private DateTime _logDate;

        private const string ErrorMessage = "can't be moved to";
        private const string ErrorLevel = "Warning";

        public SafeMoveFileTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void SafeMoveFile_ValidData_ShouldMoveDirectory()
        {
            var folderPath = $"{_logFixture.TestFolderPath}\\SafeMoveFile";
            var filePath = $"{folderPath}\\OriginalFile.cs";
            var newFilePath = $"{folderPath}\\MovedFile.cs";

            try
            {
                Directory.CreateDirectory(folderPath);
                using (var stream = File.Create(filePath))
                {
                }

                var originFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Fs.SafeMoveFile(filePath, newFilePath);

                var destinationFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Assert.True(originFolderTotalFiles == destinationFolderTotalFiles);
                Assert.False(File.Exists(filePath));
                Assert.True(File.Exists(newFilePath));
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(folderPath, true);
            }
        }

        [Theory]
        [InlineData("", "anything")]
        [InlineData(null, "anything")]
        public void SafeMoveFile_OriginFileDoesNotExist_JustReturns(string filePath, string newfilePath)
        {
            Fs.SafeMoveFile(filePath, newfilePath);
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_MovesFileSuccessfully()
        {
            var folderPath = $"{_logFixture.TestFolderPath}\\DestExists_Overwrite";
            var fileRelativePath = $"{folderPath}\\Test_Overwrite.csproj";
            var newFileRelativePath = $"{folderPath}\\Test_Overwrite_Dest.csproj";
            var sourceFilePath = Path.Combine(Environment.CurrentDirectory, fileRelativePath);
            var newFilePath = Path.Combine(Environment.CurrentDirectory, newFileRelativePath);

            try
            {
                Directory.CreateDirectory(folderPath);

                using (var stream = File.Create(sourceFilePath))
                {
                }

                using (var stream = File.Create(newFilePath))
                {
                }

                var originFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Fs.SafeMoveFile(sourceFilePath, newFilePath);

                var destinationFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Assert.True(originFolderTotalFiles > destinationFolderTotalFiles);
                Assert.False(File.Exists(sourceFilePath));
                Assert.True(File.Exists(newFilePath));
            }
            finally
            {
                // tidy up test data
                Directory.Delete(folderPath, true);
            }
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_NoOverwrite_JustReturns()
        {
            var folderPath = $"{_logFixture.TestFolderPath}\\DestFileExists";
            var fileRelativePath = $"{folderPath}\\Test.csproj";
            var newFileRelativePath = $"{folderPath}\\Test_Dest.csproj";
            var sourceFilePath = Path.Combine(Environment.CurrentDirectory, fileRelativePath);
            var newFilePath = Path.Combine(Environment.CurrentDirectory, newFileRelativePath);

            try
            {
                Directory.CreateDirectory(folderPath);

                using (var stream = File.Create(sourceFilePath))
                {
                }

                using (var stream = File.Create(newFilePath))
                {
                }

                var originFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Fs.SafeMoveFile(sourceFilePath, newFileRelativePath, false);

                var destinationFolderTotalFiles = Directory.GetFiles(folderPath).Length;

                Assert.True(originFolderTotalFiles == destinationFolderTotalFiles);
                Assert.True(File.Exists(sourceFilePath));
                Assert.True(File.Exists(newFilePath));
            }
            finally
            {
                // tidy up test data
                Directory.Delete(folderPath, true);
            }
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_NoPermissions_ShouldLogException()
        {
            var fileName = "Test.csproj";
            var folderPath = $"{_logFixture.TestFolderPath}\\DestFileExists_Overwrite_NoPermissions";
            var fileRelativePath = $"{folderPath}\\{fileName}";
            var newFileRelativePath = $"{folderPath}\\Test_Dest.csproj";
            var sourceFilePath = Path.Combine(Environment.CurrentDirectory, fileRelativePath);
            var newFilePath = Path.Combine(Environment.CurrentDirectory, newFileRelativePath);
            FileInfo newFileInfo;
            try
            {
                Directory.CreateDirectory(folderPath);

                using (var stream = File.Create(sourceFilePath))
                {
                }

                using (var stream = File.Create(newFilePath))
                {
                }

                newFileInfo = new FileInfo(newFilePath)
                {
                    IsReadOnly = true,
                };

                _logDate = DateTime.Now;
                Fs.SafeMoveFile(sourceFilePath, newFilePath);

                Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{fileName} {ErrorMessage}"));
            }
            finally
            {
                // tidy up test data
                newFileInfo = new FileInfo(newFilePath)
                {
                    IsReadOnly = false,
                };
                Directory.Delete(folderPath, true);
            }
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_NoPermissions_NoWarnOnFailure_ShouldNotLogException()
        {
            var fileName = "NoPermissions_NoWarnOnFailure.csproj";
            var folderPath = $"{_logFixture.TestFolderPath}\\DestFileExists_Overwrite_NoPermissions_NoWarnOnFailure";
            var fileRelativePath = $"{folderPath}\\Test.csproj";
            var newFileRelativePath = $"{folderPath}\\{fileName}";
            var sourceFilePath = Path.Combine(Environment.CurrentDirectory, fileRelativePath);
            var newFilePath = Path.Combine(Environment.CurrentDirectory, newFileRelativePath);
            FileInfo newFileInfo;
            try
            {
                Directory.CreateDirectory(folderPath);

                using (var stream = File.Create(sourceFilePath))
                {
                }

                using (var stream = File.Create(newFilePath))
                {
                }

                newFileInfo = new FileInfo(newFilePath)
                {
                    IsReadOnly = true,
                };

                _logDate = DateTime.Now;
                Fs.SafeMoveFile(sourceFilePath, newFilePath, true, false);

                Assert.False(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{fileName} {ErrorMessage}"));
            }
            finally
            {
                // tidy up test data
                newFileInfo = new FileInfo(newFilePath)
                {
                    IsReadOnly = false,
                };
                Directory.Delete(folderPath, true);
            }
        }
    }
}
