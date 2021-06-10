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
    public class SafeDeleteFileTests
    {
        private readonly LogFixture _logFixture;
        private readonly string _dirPath;
        private string _filePath;

        private DateTime _logDate;
        private const string ErrorMessage = "can't be delete";
        private const string ErrorLevel = "Warning";

        public SafeDeleteFileTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            _dirPath = $"{_logFixture.TestFolderPath}\\SafeDelete";
            _filePath = Path.Combine(Environment.CurrentDirectory, _dirPath, "Test.csproj");

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void SafeDeleteFile_ExistingFile_ShouldHaveDeletedFile()
        {
            var testDir = $"{_dirPath}ExistingFile";
            try
            {
                Directory.CreateDirectory(testDir);

                _filePath = Path.Combine(testDir, "Test.csproj");

                using (var stream = File.Create(_filePath))
                {
                }

                Fs.SafeDeleteFile(_filePath);

                Assert.False(File.Exists(_filePath));
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(testDir, true);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SafeDeleteFile_PathNotFound_ShouldNotThrowException(string filePath)
        {
            Fs.SafeDeleteFile(filePath);
        }

        [Fact]
        public void SafeDeleteFile_NoPermissions_ShouldHandleException()
        {
            var testDir = $"{_dirPath}NoPermissions";
            Directory.CreateDirectory(testDir);
            _filePath = Path.Combine(Environment.CurrentDirectory, testDir, "FileWitehNoPermissions.csproj");

            using (var stream = File.Create(_filePath))
            {
            }

            var fileInfo = new FileInfo(_filePath)
            {
                IsReadOnly = true,
            };

            _logDate = DateTime.Now;
            Fs.SafeDeleteFile(_filePath);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
            fileInfo.IsReadOnly = false;
            Directory.Delete(testDir, true);
        }
    }
}
