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
    public class EnsureFileEditableTests
    {
        private readonly LogFixture _logFixture;

        private DateTime _logDate;
        private const string ErrorMessage = "Cannot remove readonly protection from file";
        private const string ErrorLevel = "Warning";

        public EnsureFileEditableTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void EnsureFileEditable_FileIsReadOnly_ShouldChangeToReadOnly()
        {
            var folderPath = $"{_logFixture.TestFolderPath}\\TestProject";
            var filePath = $"{folderPath}\\ProtectedTest.csproj";
            Directory.CreateDirectory(folderPath);

            var fileInfo = new FileInfo(filePath);
            FileInfo newFileInfo;
            try
            {
                using var stream = File.Create(filePath);
                fileInfo.IsReadOnly = true;

                Fs.EnsureFileEditable(filePath);
                newFileInfo = new FileInfo(filePath);

                Assert.False(newFileInfo.IsReadOnly);
            }
            finally
            {
                // tidy up testing data
                newFileInfo = new FileInfo(filePath)
                {
                    IsReadOnly = false,
                };

                Directory.Delete(folderPath, true);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EnsureFileEditable_PathNotFound_ShouldLogError(string filePath)
        {
            _logDate = DateTime.Now;

            Fs.EnsureFileEditable(filePath);

            Assert.True(_logFixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void EnsureFileEditable_FileIsNotReadOnly_ShouldNotModifyIsReadOnly()
        {
            var filePath = $"{_logFixture.TestFolderPath}\\TestProject\\Test.csproj";
            var originalFileInfo = new FileInfo(filePath);
            Fs.EnsureFileEditable(filePath);
            var newFileInfo = new FileInfo(filePath);

            Assert.Equal(newFileInfo.IsReadOnly, originalFileInfo.IsReadOnly);
        }
    }
}
